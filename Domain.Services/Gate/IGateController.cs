namespace EtGate.Domain.Services.Gate;

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
    LANG_FRAUD_A,
    LANG_FRAUD_B,
    LANG_FRAUD_DISAPPEARANCE,
    LANG_FRAUD_HOLDING = 0x08,
    LANG_FRAUD_JUMP,
    LANG_FRAUD_RAMPING,
    LANG_FRAUD_UNEXPECTED_MOTION,
}

// TODO: trim it according to the domain requirements
public enum IntrusionType
{
    LANG_INTRUSION_A,
    LANG_INTRUSION_B,
    LANG_OPPOSITE_INTRUSION_A,
    LANG_OPPOSITE_INTRUSION_B,
    LANG_PREALARM_A,
    LANG_PREALARM_B
}

public enum ePassageTimeouts
{
    LANG_ENTRY_TIMEOUT_A = 0, //A passenger coming from the entrance (A side) did not cross the gate in the allotted time
    LANG_ENTRY_TIMEOUT_B = 1, //A passenger coming from the entrance (B side) did not cross the gate in the allotted time
    LANG_EXIT_TIMEOUT = 2, // The exit has not been cleared completely in the allotted time
    LANG_NO_CROSSING_TIMEOUT = 4, //A passenger coming did not cross the gate in the allotted time
    LANG_NO_ENTRY_TIMEOUT = 8, // Timeouts during boarding (the person did not enter the gate in the allotted time)
    LANG_SECURITY_TIMEOUT = 16, //A passenger took too much time to exit the safety zone and prevents the closure of the doorsa
    LANG_VALIDATION_TIMEOUT = 32
}

public record Fraud(bool bEntry, List<FraudType> fraudType // don't know that we can have multiple types of Frauds at once. The code suggests this.
    );

// It is very easy to have intrusions from both sides (if both sides are controlled). Just make on both sides stand a person.
// TODO: for now, the domain is made around IER. This of course is bad. But can be corrected only after experimentation.
public record struct IntrusionX(
    bool LANG_INTRUSION_A,
    bool LANG_INTRUSION_B,
    bool LANG_OPPOSITE_INTRUSION_A,
    bool LANG_OPPOSITE_INTRUSION_B,
    bool LANG_PREALARM_A,
    bool LANG_PREALARM_B)
{
    bool bEntry => LANG_INTRUSION_A || LANG_OPPOSITE_INTRUSION_A || LANG_PREALARM_A;
    bool bExit => LANG_INTRUSION_B || LANG_OPPOSITE_INTRUSION_B || LANG_PREALARM_B;
}

public record OpenDoor(bool bEntry);
public record CloseDoor(bool bEntry);
public record WaitForAuthroization;

// TODO: IGateController doesn't seem necessary. At least it need not derive from these three interfaces.
public interface IGateController
{
    // the application is supposed to save the Demanded state. It may be possible that the device is not reachable now, but as soon as it is reachable, the command would be fulfilled.

    bool Reboot(bool bHardboot);

    //RawEventsInNominalMode RawEvtsInIdleModeObservable { get; }
}
