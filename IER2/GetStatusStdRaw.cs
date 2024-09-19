using IFS2.Equipment.HardwareInterface.IERPLCManager;
using LanguageExt;

namespace EtGate.IER;

public enum eDoorsStatesMachine
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
    public Option<eDoorsStatesMachine> exceptionMode;
    public Option<eDoorNominalModes> stateIfInNominal;
    public Option<int> customersActive;
    public bool IsCustomerActive(int id) // id starts from 1
    { return customersActive.Match(x => (x & (1 << (id - 1))) != 0, () => false); }
    public Option<bool> emergencyButton;
    public Option<bool> entryLockOpen;
    public Option<bool> systemPoweredByUPS;
    public List<eIERPLCErrors> majorTechnicalFailures;
    public List<eIERPLCErrors> minorTechnicalFailures;

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


public struct IerErrors
{
    public List<eIERPLCErrors> majorTechnicalFailures;
    public List<eIERPLCErrors> minorTechnicalFailures;
}

public struct IerNominalMode
{
    public Option<int> AuthorizationA;
    public Option<int> AuthorizationB;
    public Option<int> PassageA;
    public Option<int> PassageB;
    public Option<int> peopleDetectedInsideA;
    public Option<int> peopleDetectedInsideB;
    public Option<int> presencesDetectedInsideA;
    public Option<int> presencesDetectedInsideB;
    public Option<int> readerLockedSideA;
    public Option<int> readerLockedSideB;
    public Option<bool> safety_zone;
    public Option<bool> safety_zone_A;
    public Option<bool> safety_zone_B;
    public Option<eDoorNominalModes> stateIfInNominal;
    public Option<int> customersActive;
    public bool IsCustomerActive(int id) // id starts from 1
    { return customersActive.Match(x => (x & (1 << (id - 1))) != 0, () => false); }
    public Option<int> timeouts;
    public Option<int> infractions;
}

// TODO: doubtful where to keep these fields. may be they are applicable to the nominal mode also
public struct IerDoors
{
    public Option<bool> brakeEnabled;
    public Option<bool> doorFailure;
    public Option<bool> doorinitialized;

    public Option<int> doorCurrentMovementOfObstacle;
    public Option<bool> door_unexpected_motion;
}



//public static IerOverallStatus ToOverallStatus(this GetStatusStdRaw raw)=>mapperOverallStatus.Map<IerOverallStatus>(raw);
//public static IerErrors ToErrors(this GetStatusStdRaw raw) => mapperErrors.Map<IerErrors>(raw);
//public static IerNominalMode ToNominalMode(this GetStatusStdRaw raw) => mapperNominalMode.Map<IerNominalMode>(raw);
//public static IerDoors ToDoors(this GetStatusStdRaw raw) => mapperDoors.Map<IerDoors>(raw);
