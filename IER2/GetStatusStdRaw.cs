using LanguageExt;

namespace EtGate.IER;

public enum OverallState
{
    EGRESS,
    NOMINAL, //Normal mode .. if set then check for doors Nominal Mode 
    EMERGENCY,
    MAINTENANCE,
    LOCKED_OPEN,// FORCED_OPEN,
    POWER_DOWN,
    TECHNICAL_FAILURE //OUTOFORER
}

public enum SideOperatingMode
{
    Closed,
    Controlled,
    Free,    
}

public enum DoorsMode
{
    LockClosed,
    NormallyClosed,
    NormallyOpenedA,
    NormallyOpenedB,
    OpticalA,
    OpticalB,
    LockedOpenA,
    LockedOpenB
};

public struct GetStatusStdRaw
{
    public Option<int> AuthorizationA;
    public Option<int> AuthorizationB;
    public Option<int> PassageA;
    public Option<int> PassageB;
    public Option<DoorsMode> door_mode;
    public Option<SideOperatingMode> operatingModeSideA;
    public Option<SideOperatingMode> operatingModeSideB;
    public Option<int> peopleDetectedInsideA;
    public Option<int> peopleDetectedInsideB;
    public Option<int> presencesDetectedInsideA;
    public Option<int> presencesDetectedInsideB;
    public Option<int> readerLockedSideA;
    public Option<int> readerLockedSideB;
    public Option<OverallState> exceptionMode;
    public Option<eDoorNominalModes> stateIfInNominal;
    public Option<int> customersActive;
    public bool IsCustomerActive(int id) // id starts from 1
    { return customersActive.Match(x => (x & (1 << (id - 1))) != 0, () => false); }
    public Option<bool> emergencyButton;
    public Option<bool> entryLockOpen;
    public Option<bool> systemPoweredByUPS;
    public List<eIERPLCError> majorTechnicalFailures;
    public List<eIERPLCError> minorTechnicalFailures;

    public Option<int> timeouts;
    public Option<int> infractions;

    public Option<bool> brakeEnabled;
    public Option<bool> doorFailure;
    public Option<bool> doorinitialized;
    public Option<bool> safety_zone;
    public Option<bool> safety_zone_A;
    public Option<bool> safety_zone_B;
    public Option<int> doorCurrentMovementOfObstacle;
    public Option<bool> door_unexpected_motion;
}

// TODO: This unnecessary structure is because I am not 100% sure that GetStatusStdRaw returns all the fields.
// If it does, then we can remove GetStatusStdRaw with this structure
public struct GetStatusStdRawComplete
{
    public int AuthorizationA;
    public int AuthorizationB;
    public int PassageA;
    public int PassageB;
    public DoorsMode door_mode;
    public SideOperatingMode operatingModeSideA;
    public SideOperatingMode operatingModeSideB;
    public int peopleDetectedInsideA;
    public int peopleDetectedInsideB;
    public int presencesDetectedInsideA;
    public int presencesDetectedInsideB;
    public int readerLockedSideA;
    public int readerLockedSideB;
    public OverallState exceptionMode;
    public eDoorNominalModes stateIfInNominal;
    public int customersActive;
    public bool IsCustomerActive(int id) // id starts from 1
    { return (customersActive & (1 << (id - 1))) != 0; }
    public bool bEmergencyButton;
    public bool bEntryLockOpen;
    public bool bSystemPoweredByUPS;
    public List<eIERPLCError> majorTechnicalFailures;
    public List<eIERPLCError> minorTechnicalFailures;
    public int timeouts;
    public int infractions;
    public bool brakeEnabled;
    public bool doorFailure;
    public bool doorinitialized;
    public bool safety_zone;
    public bool safety_zone_A;
    public bool safety_zone_B;
    public int doorCurrentMovementOfObstacle;
    public bool door_unexpected_motion;
    }


public struct IerErrors
{
    public List<eIERPLCError> majorTechnicalFailures;
    public List<eIERPLCError> minorTechnicalFailures;
}

public struct IerNominalMode
{
    public int AuthorizationA;
    public int AuthorizationB;
    public int PassageA;
    public int PassageB;
    public int peopleDetectedInsideA;
    public int peopleDetectedInsideB;
    public int presencesDetectedInsideA;
    public int presencesDetectedInsideB;
    public int readerLockedSideA;
    public int readerLockedSideB;
    public bool safety_zone;
    public bool safety_zone_A;
    public bool safety_zone_B;
    public eDoorNominalModes stateIfInNominal;
    public int customersActive;
    public bool IsCustomerActive(int id) // id starts from 1
        => (customersActive & (1 << (id - 1))) != 0;
    public int timeouts;
    public int infractions;
}

// TODO: doubtful where to keep these fields. may be they are applicable to the nominal mode also
public struct IerDoors
{
    public bool brakeEnabled;
    public bool doorFailure;
    public bool doorinitialized;

    public int doorCurrentMovementOfObstacle;
    public bool door_unexpected_motion;
}



//public static IerOverallStatus ToOverallStatus(this GetStatusStdRaw raw)=>mapperOverallStatus.Map<IerOverallStatus>(raw);
//public static IerErrors ToErrors(this GetStatusStdRaw raw) => mapperErrors.Map<IerErrors>(raw);
//public static IerNominalMode ToNominalMode(this GetStatusStdRaw raw) => mapperNominalMode.Map<IerNominalMode>(raw);
//public static IerDoors ToDoors(this GetStatusStdRaw raw) => mapperDoors.Map<IerDoors>(raw);
