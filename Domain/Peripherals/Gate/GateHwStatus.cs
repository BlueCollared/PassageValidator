using Domain.Peripherals.Qr;
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

    // TODO: since one of eDoorsStatesMachine is POWER_DOWN, it is possible that we might have to remove `bConnected`
    public record GateHwStatus(bool bConnected, Option<eDoorsStatesMachine> doorState) : ModuleStatus
    {
        public static GateHwStatus Disconnected => new GateHwStatus(bConnected: false, Option<eDoorsStatesMachine>.None);
        public static GateHwStatus AllGood => new GateHwStatus(bConnected: true, eDoorsStatesMachine.NOMINAL);

        public override bool IsAvailable =>bConnected && doorState == eDoorsStatesMachine.NOMINAL;
    }
}