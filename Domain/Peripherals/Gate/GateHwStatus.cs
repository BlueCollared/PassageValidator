using EtGate.Domain;

namespace Domain.Peripherals.Passage
{
    public record GateHwStatus(bool bConnected, bool bWorking) : ModuleStatus
    {
        public override bool IsAvailable => throw new NotImplementedException();
    }
}