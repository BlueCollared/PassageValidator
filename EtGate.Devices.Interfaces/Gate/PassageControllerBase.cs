using Domain.Peripherals.Passage;
using EtGate.Domain.Passage.PassageEvts;
using EtGate.Domain.Services.Gate;
using IFS2.Equipment.DriverInterface;
using OneOf;

namespace EtGate.Devices.Interfaces.Gate;

public class PassageControllerBase : StatusStreamBase<GateHwStatus>, IPassageControllerEx
{
    public record Fraud(bool bEntry, List<FraudType> fraudType // don't know that we can have multiple types of Frauds at once
    );
    public record Intrusion(bool bEntry);
    public record Idle_NoAuthorizationPending;    

    public IObservable<GateHwStatus> GateStatusObservable => throw new NotImplementedException();

    public IObservable<OneOf<Intrusion, Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone>> PassageStatusObservable 
        => throw new NotImplementedException();

    IObservable<OneOf<Domain.Passage.PassageEvts.Intrusion, Domain.Passage.PassageEvts.Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone>> 
        IPassageController.PassageStatusObservable => throw new NotImplementedException();

    public bool Authorize(int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public void Reboot()
    {
        throw new NotImplementedException();
    }

    public void SetMode(eSideOperatingModeGate entry, eSideOperatingModeGate exit, DoorsMode doorsMode)
    {
        throw new NotImplementedException();
    }

    public void SetOpMode(OpMode mode)
    {
        throw new NotImplementedException();
    }

    public enum FraudType
    {
        Disappearance,
        Holding,
        Jump,
        Ramping,
        UnexpectedMotion
    }
    //public record Intrusion(bool bEntry, List<
}
