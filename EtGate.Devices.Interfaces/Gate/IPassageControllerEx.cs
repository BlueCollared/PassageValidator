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


global using RawEventsInNominalMode = OneOf.OneOf<
    EtGate.Devices.Interfaces.Gate.Intrusion,
    EtGate.Devices.Interfaces.Gate.Fraud,
    EtGate.Devices.Interfaces.Gate.OpenDoor,
    EtGate.Devices.Interfaces.Gate.WaitForAuthroization,
    EtGate.Devices.Interfaces.Gate.CloseDoor>;

using EtGate.Domain.Services.Gate;

namespace EtGate.Devices.Interfaces.Gate;
enum eDoorNominalModes //Normal case
{
    OPEN_DOOR, //Doors are opened
    CLOSE_DOOR, //Doors are closed
    FRAUD, //A Fraud has occured
    INTRUSION, // Intrusion
    WAIT_FOR_AUTHORIZATION //The gate is ready to accept a new authorisation to start a boarding (or for a person to enter the gate)
}

public enum FraudType
{
    Disappearance,
    Holding,
    Jump,
    Ramping,
    UnexpectedMotion
}

public record Fraud(bool bEntry, List<FraudType> fraudType // don't know that we can have multiple types of Frauds at once. The code suggests this.
    );

public record Intrusion(bool bEntry);
public record OpenDoor(bool bEntry);
public record CloseDoor(bool bEntry);
public record WaitForAuthroization;

public interface IPassageControllerEx : IPassageController, IGateModeController
{
    // the application is supposed to save the Demanded state. It may be possible that the device is not reachable now, but as soon as it is reachable, the command would be fulfilled.

    bool Reboot(bool bHardboot);

    //RawEventsInNominalMode RawEvtsInIdleModeObservable { get; }
}
