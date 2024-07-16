using Domain.Peripherals.Passage;
using OneOf;

namespace Domain.InService
{
    public interface IPassageManager
    {
        IObservable<OneOf<Intrusion, Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone>> PassageStatusObservable { get; }

        // returns true if the request is accepted

        // {`ticketId`, authorizationId} would be bounced back in the reply when the passage is done/not done/intrusion on other side
        bool Authorize(string ticketId, int nAuthorizations);
    }
}