namespace EtGate.Domain.Services.Gate;

public interface IPassageManager
{
    // It is kept general i.e. Idle Status i.e. w/o any authorizations submitted. For our case, we have Free Exit, so possible values include PassageDone are applicable.
    // But if there is no free exit, then such value would not pop.
    //ObsIdleEvents IdleStatusObservable { get; }

    // returns true if the request is accepted

    // {`ticketId`, authorizationId} would be bounced back in the reply when the passage is done/not done/intrusion on other side
    ObsAuthEvents Authorize(string ticketId, int nAuthorizations);
}