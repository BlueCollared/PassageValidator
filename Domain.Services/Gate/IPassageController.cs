global using EventInNominalMode = OneOf.OneOf<
    EtGate.Devices.Interfaces.Gate.Intrusion,
    EtGate.Devices.Interfaces.Gate.Fraud,
    EtGate.Devices.Interfaces.Gate.OpenDoor,
    EtGate.Devices.Interfaces.Gate.WaitForAuthroization,
    EtGate.Devices.Interfaces.Gate.CloseDoor>;

namespace EtGate.Devices.Interfaces.Gate
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
