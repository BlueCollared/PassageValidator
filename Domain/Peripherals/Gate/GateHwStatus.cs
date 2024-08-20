namespace Domain.Peripherals.Passage
{
    public record GateHwStatus(bool bConnected, string firmwareVersion, bool bWorking);
}