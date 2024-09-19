using EtGate.Domain;
using LanguageExt;

namespace Domain.Peripherals.Passage
{
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

    public record GateHwStatus_(eDoorsStatesMachine doorState, bool bEmergencyButton)
    {
        public static GateHwStatus_ AllGood => new GateHwStatus_(eDoorsStatesMachine.NOMINAL, bEmergencyButton: false);
    }
        // TODO: since one of eDoorsStatesMachine is POWER_DOWN, it is possible that we might have to remove `bConnected`
    public record GateHwStatus(bool bConnected, Option<GateHwStatus_> status) : ModuleStatus
    {
        public static GateHwStatus Disconnected => new GateHwStatus(bConnected: false, status : default);
        public static GateHwStatus AllGood => new GateHwStatus(bConnected: true, status: GateHwStatus_.AllGood);

        public override bool IsAvailable =>this == AllGood;
    }
}