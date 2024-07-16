using Domain.InService;
using Domain.Peripherals.Passage;
using OneOf;

namespace Domain
{
    public enum PassageEvtTypes
    {
        EntryPassed,
        ExitPassed,
        EntryPassageTimeout,
        ExitPassageTimeout,
        EntryIntrusion,
        ExitIntrusion
    }

    public class PassageMgr : IPassageManager
    {
        public IObservable<OneOf<Intrusion, Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone>> PassageStatusObservable => throw new NotImplementedException();

        public bool Authorize(string ticketId, int nAuthorizations)
        {
            throw new NotImplementedException();
        }
    }
}
