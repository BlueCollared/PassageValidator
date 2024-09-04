namespace EtGate.Domain.Services.Gate;


using ObsAuthEvents = System.IObservable<OneOf.OneOf<
    EtGate.Domain.Passage.PassageEvts.Intrusion,
    EtGate.Domain.Passage.PassageEvts.Fraud,
    EtGate.Domain.Passage.PassageEvts.OpenDoor,
    EtGate.Domain.Passage.PassageEvts.PassageTimeout,
    EtGate.Domain.Passage.PassageEvts.AuthroizedPassengerSteppedBack,
    EtGate.Domain.Passage.PassageEvts.PassageDone>>;

using ObsIdleEvents = System.IObservable<OneOf.OneOf<
    EtGate.Domain.Passage.IdleEvts.Intrusion,
    EtGate.Domain.Passage.IdleEvts.Fraud,
    EtGate.Domain.Passage.IdleEvts.PassageTimeout,
    EtGate.Domain.Passage.IdleEvts.PassageDone>>;

public class PassageMgr : IPassageManager
{
    private readonly IPassageController controller;

    public PassageMgr()
    {
        throw new NotImplementedException();
    }

    public PassageMgr(IPassageController controller)
    {
        this.controller = controller;
    }

    public ObsIdleEvents IdleStatusObservable => throw new NotImplementedException();

    public ObsAuthEvents Authorize(string ticketId, int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}