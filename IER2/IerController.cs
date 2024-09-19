using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate;
using EtGate.IER;
using LanguageExt;
using OneOf;

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

    //public override IObservable<GateHwStatus> GateStatusObservable => 
    //    //base.GateStatusObservable.Select(x=>new GateHwStatus(x.bConnected, x.doorState));
    //    ier2.StatusObservable.Select(x=>new GateHwStatus(x.IsRight, x.Match(r=>r
    //    ).IsRight && x.Right.));

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

    public override bool SetNormalMode(Option<global::Domain.Peripherals.Passage.SideOperatingMode> entry, Option<global::Domain.Peripherals.Passage.SideOperatingMode> exit)
    {
        throw new NotImplementedException();
    }
}
