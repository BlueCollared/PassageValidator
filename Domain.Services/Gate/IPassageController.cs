global using EventInNominalMode = OneOf.OneOf<
    EtGate.Domain.Services.Gate.Intrusion,
    EtGate.Domain.Services.Gate.Fraud,
    EtGate.Domain.Services.Gate.OpenDoor,
    EtGate.Domain.Services.Gate.WaitForAuthroization,
    EtGate.Domain.Services.Gate.CloseDoor>;

namespace EtGate.Domain.Services.Gate
{
    // It is possible that around the time authroization is submitted, intrusion gets detected.
    // It is its job to suppress raising the Intrusion (by first making sure that Intrusion has really removed)

    public interface IPassageController
    {
        IObservable<EventInNominalMode> PassageStatusObservable { get; }

        // returns true if the request is accepted    
        bool Authorize(int nAuthorizations);
    }
}
