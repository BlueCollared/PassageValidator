global using ObsAuthEvents = System.IObservable<OneOf.OneOf<
    EtGate.Domain.Passage.PassageEvts.Intrusion, 
    EtGate.Domain.Passage.PassageEvts.Fraud, 
    EtGate.Domain.Passage.PassageEvts.OpenDoor, 
    EtGate.Domain.Passage.PassageEvts.PassageTimeout, 
    EtGate.Domain.Passage.PassageEvts.AuthroizedPassengerSteppedBack, 
    EtGate.Domain.Passage.PassageEvts.PassageDone>>;

global using ObsIdleEvents = System.IObservable<OneOf.OneOf<
    EtGate.Domain.Passage.IdleEvts.Intrusion, 
    EtGate.Domain.Passage.IdleEvts.Fraud, 
    EtGate.Domain.Passage.IdleEvts.PassageTimeout, 
    EtGate.Domain.Passage.IdleEvts.PassageDone>>;

namespace EtGate.Domain.Services.Gate;

public interface IPassageManager
{
    // It is kept general i.e. Idle Status i.e. w/o any authorizations submitted. For our case, we have Free Exit, so possible values include PassageDone are applicable.
    // But if there is no free exit, then such value would not pop.
    ObsIdleEvents IdleStatusObservable { get; }

    // returns true if the request is accepted

    // {`ticketId`, authorizationId} would be bounced back in the reply when the passage is done/not done/intrusion on other side
    ObsAuthEvents Authorize(string ticketId, int nAuthorizations);
}