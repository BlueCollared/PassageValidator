using Domain.Peripherals.Passage;
using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate;
using EtGate.IER;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using OneOf;
using System.Reactive.Linq;

namespace EtGate.Devices.IER;

public class IerController : GateControllerBase
{
    bool bIsConnected => CurStatus != null && CurStatus.bConnected;
    bool bIsAvailable => CurStatus != null && CurStatus.IsAvailable;    

    Ier_To_DomainAdapter ier;
    private readonly IIerStatusMonitor ier2;

    public IerController(IIerStatusMonitor xmlRpc, IIerXmlRpc xmlRpc2)
    {
        ier = new Ier_To_DomainAdapter(
            xmlRpc2
            );
        ier2 = xmlRpc;
        // ideally it belongs to IDeviceStatus, but can't keep it there.
        statusObservable.Subscribe(x => CurStatus = x);
    }

    public IObservable<Either<IERApiError, GetStatusStdRawComplete>> comp => ier2.StatusObservable
            .Select(x => x.Bind<GetStatusStdRawComplete>(y => y.To_GetStatusStdRawComplete()));

    public override IObservable<GateHwStatus> statusObservable =>
          comp
            .Select(x => x.Match(
                r => new GateHwStatus(true, r.To_GateHwStatus()),
                l => new GateHwStatus(l != IERApiError.DeviceInaccessible) // TODO: still to treat errors other than DeviceInaccessible
                ));
    
    public override IObservable<EventInNominalMode> InServiceEventsObservable
        => comp.Where(x => x.IsRight)
        .Select<Either<IERApiError, GetStatusStdRawComplete>, GetStatusStdRawComplete>(a => a.Value())
        .Where(a => a.exceptionMode == OverallState.NOMINAL)
        .Select<GetStatusStdRawComplete,
            OneOf<IntrusionX, Fraud, OpenDoor, WaitForAuthroization, CloseDoor>
            >(a =>
            // TODO: correct it
            a.stateIfInNominal switch
            {
                eDoorNominalModes.CLOSE_DOOR => new CloseDoor(bEntry: true),
                eDoorNominalModes.FRAUD => new Fraud(bEntry: true, null),
                eDoorNominalModes.OPEN_DOOR => new OpenDoor(bEntry: true),
                eDoorNominalModes.WAIT_FOR_AUTHORIZATION => new WaitForAuthroization(),
                eDoorNominalModes.INTRUSION => a.To_IntrusionX()
            });

    public override bool Authorize(int nAuthorizations, bool bEntry)
    {
        if (!bIsConnected)
            return false;
        return ier.Authorise(nAuthorizations, bEntry);
    }

    public override Option<DateTimeOffset> GetDate()
    {
        if (!bIsConnected)
            return Option<DateTimeOffset>.None;
        return ier.GetDate().Map(x=>new DateTimeOffset(x)); // TODO: see if we can keep `DateTimeOffset` acrosss the code instead of making this conversion
    }

    public override bool Reboot(bool bHardboot)
    {
        if (!bIsConnected)
            return false;

        if (bHardboot)
            return ier.Reboot();
        else
            return ier.Restart();
    }

    public override bool SetDate(DateTimeOffset dt)
    {
        if (!bIsConnected)
            return false;
        return ier.SetDate(dt.DateTime); // TODO: see if we can keep `DateTimeOffset` acrosss the code instead of making this conversion
    }    

    public override bool SetEmergency()
    {
        if (!bIsConnected)
            return false;
        return ier.SetEmergency();
    }

    public override bool SetMaintenance()
    {
        if (!bIsConnected)
            return false;
        return ier.SetMaintenance();
    }

    public override bool SetNormalMode(GateOperationConfig config)
    {
        if (!bIsConnected)
            return false;
        return ier.SetNormalMode(config);        
    }

    public override bool SetOOS()
    {
        if (!bIsConnected)
            return false;

        return ier.SetOOS();
    }
}
