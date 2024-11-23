using Equipment.Core;

//using IFS2.Equipment.DriverInterface;
using LanguageExt;

namespace Domain.Peripherals.Passage;

//public enum DoorsMode
//{
//    NormallyClosed,
//    NormallyOpenedA,
//    NormallyOpenedB,
//    Other
//};

//public struct GateOverallStatus
//{
//    public Option<DoorsStatesMachine> exceptionMode;
//    public Option<bool> emergencyButton;
//    public Option<bool> entryLockOpen;
//    public Option<bool> systemPoweredByUPS;
//}

//public struct GateConfiguration
//{
//    public Option<DoorsMode> door_mode;
//    public Option<bool> bIsEntryFree;
//    public Option<bool> bIsExitFree;
//    //public Option<SideOperatingMode> operatingModeSideA;
//    //public Option<SideOperatingMode> operatingModeSideB;
//}

public enum GatePhysicalState
{
    NOMINAL, //Normal mode .. if set then check for doors Nominal Mode 
    EMERGENCY,
    MAINTENANCE,
    //LOCKED_OPEN,// FORCED_OPEN,
    //POWER_DOWN, // TODO: see if it can be categorized as OOO
    OOO
}

public record GateHwStatus_(
    GatePhysicalState mode,
    bool bEmergencyButton,
    bool bEntryLockOpen, // TODO: can be removed because domain isn't interested in it. Very likely, exception mode is set to LOCKED_OPEN when this happens, which gets transformed to OOO state
    bool bSystemPoweredByUPS
)
{
    public static GateHwStatus_ AllGood => new GateHwStatus_(GatePhysicalState.NOMINAL, bEmergencyButton: false, bEntryLockOpen:false, 
        bSystemPoweredByUPS:false // TODO: this is not entirely good. It is possible that the system is powered by UPS but a passage is in transit. We need to make sure that this passage completes
        );
}
    // TODO: since one of eDoorsStatesMachine is POWER_DOWN, it is possible that we might have to remove `bConnected`
public record GateHwStatus(bool bConnected, Option<GateHwStatus_> status = default) : ModuleStatus
{
    public static GateHwStatus Disconnected => new GateHwStatus(bConnected: false, status : default);
    public static GateHwStatus AllGood => new GateHwStatus(bConnected: true, status: GateHwStatus_.AllGood);

    public override bool IsAvailable =>this == AllGood;

    public static GateHwStatus DisConnected => new GateHwStatus(bConnected: false);

    //public ModuleStatus defStatus => Disconnected;
}