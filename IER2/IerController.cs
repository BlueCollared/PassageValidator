using Domain.Peripherals.Passage;
using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate;
using EtGate.IER;
using LanguageExt;
using OneOf;
using System.Reactive.Linq;

namespace EtGate.Devices.IER;

public class IerController : GateControllerBase
{
    bool bIsConnected = false;
    Ier_To_DomainAdapter ier;
    private readonly IIerStatusMonitor ier2;

    public IerController(IIerStatusMonitor xmlRpc, IIerXmlRpc xmlRpc2)
    {
        ier = new Ier_To_DomainAdapter(
            xmlRpc2
            );
        ier2 = xmlRpc;
    }

    public IObservable<Either<IERApiError, GetStatusStdRawComplete>> comp => ier2.StatusObservable
            .Select(x => x.Bind<GetStatusStdRawComplete>(y => y.To_GetStatusStdRawComplete()));

    public override IObservable<GateHwStatus> GateStatusObservable =>
          //ier2.StatusObservable
          //  .Select(x => x.Bind<GetStatusStdRawComplete>(y => y.To_GetStatusStdRawComplete()))
          comp
            .Select(x => x.Match(
                r => new GateHwStatus(true, r.To_GateHwStatus()),
                l => new GateHwStatus(false)
                ));    

    // TODO:
    public override IObservable<OneOf<Intrusion, Fraud, OpenDoor, WaitForAuthroization, CloseDoor>> PassageStatusObservable 
        => base.PassageStatusObservable;

    public override bool Authorize(int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public override Option<DateTimeOffset> GetDate()
    {
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
        throw new NotImplementedException();
    }
}
