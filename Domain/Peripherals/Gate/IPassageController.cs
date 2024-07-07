using OneOf;
namespace Domain.Peripherals.Passage
{
    // each of below may be embellished with several fields
    public record Intrusion;
    public record Fraud;
    public record PassageInProgress;
    public record PassageDone;

    // It is possible that around the time authroization is submitted, intrusion gets detected.
    // It is its job to suppress raising the Intrusion (by first making sure that Intrusion has really removed)
    public interface IPassageController : IPeripheral
    {
        IObservable<GateStatus> GateStatusObservable { get; }

        // TODO: not yet sure that whether I should be responsible for tracking the individual passages (by also bouncing back the {`ticketId`, authorizationId})
        // or should simply leave it to the client.
        // can change in later versions
        IObservable<OneOf<Intrusion, Fraud, PassageInProgress, PassageDone>> PassageStatusObservable { get; }
        
        // returns true if the request is accepted

        // {`ticketId`, authorizationId} would be bounced back in the reply when the passage is done/not done/intrusion on other side
        bool Authorize(string ticketId, int nAuthorizations); 
    }
}