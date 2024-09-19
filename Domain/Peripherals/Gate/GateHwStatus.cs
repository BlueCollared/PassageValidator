using EtGate.Domain;

//using IFS2.Equipment.DriverInterface;
using LanguageExt;

namespace Domain.Peripherals.Passage;

public enum DoorsStatesMachine
{        
    NOMINAL, //Normal mode .. if set then check for doors Nominal Mode 
    EMERGENCY,
    MAINTENANCE,
    LOCKED_OPEN,// FORCED_OPEN,
    POWER_DOWN, // TODO: see if it can be categorized as OOO
    OOO
}

public enum SideOperatingMode
{
    Closed,
    Controlled,
    Free
}

public enum DoorsMode
{        
    NormallyClosed,
    NormallyOpenedA,
    NormallyOpenedB,
    Other
};

public struct GateOverallStatus
{
    public Option<DoorsStatesMachine> exceptionMode;
    public Option<bool> emergencyButton;
    public Option<bool> entryLockOpen;
    public Option<bool> systemPoweredByUPS;
}

public struct GateConfiguration
{
    public Option<DoorsMode> door_mode;
    public Option<SideOperatingMode> operatingModeSideA;
    public Option<SideOperatingMode> operatingModeSideB;
}

public record GateHwStatus_(DoorsStatesMachine doorState, bool bEmergencyButton)
{
    public static GateHwStatus_ AllGood => new GateHwStatus_(DoorsStatesMachine.NOMINAL, bEmergencyButton: false);
}
    // TODO: since one of eDoorsStatesMachine is POWER_DOWN, it is possible that we might have to remove `bConnected`
public record GateHwStatus(bool bConnected, Option<GateHwStatus_> status) : ModuleStatus
{
    public static GateHwStatus Disconnected => new GateHwStatus(bConnected: false, status : default);
    public static GateHwStatus AllGood => new GateHwStatus(bConnected: true, status: GateHwStatus_.AllGood);

    public override bool IsAvailable =>this == AllGood;
}