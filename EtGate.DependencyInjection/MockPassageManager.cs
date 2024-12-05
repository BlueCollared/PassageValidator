using EtGate.Domain.Services.Gate;
using OneOf;

namespace EtGate.DependencyInjection.Mocks;

using ObsAuthEvents = System.IObservable<OneOf.OneOf<EtGate.Domain.Passage.PassageEvts.Intrusion, EtGate.Domain.Passage.PassageEvts.Fraud, EtGate.Domain.Passage.PassageEvts.OpenDoor, EtGate.Domain.Passage.PassageEvts.PassageTimeout, EtGate.Domain.Passage.PassageEvts.AuthroizedPassengerSteppedBack, EtGate.Domain.Passage.PassageEvts.PassageDone>>;

public class MockPassageManager : IGateInServiceController
{
    public IObservable<OneOf<IntrusionX, Fraud, OpenDoor, WaitForAuthroization, CloseDoor>> InServiceEventsObservable => throw new NotImplementedException();

    //IObservable<OneOf<Domain.Passage.IdleEvts.Intrusion, Domain.Passage.IdleEvts.Fraud, PassageTimeout, PassageDone>> IPassageManager.IdleStatusObservable => throw new NotImplementedException();

    public ObsAuthEvents Authorize(string ticketId, int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public bool Authorize(int nAuthorizations, bool bEntry)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
