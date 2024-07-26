namespace Domain.Peripherals.Passage
{
    public record GateStatus(bool bConnected, string firmwareVersion, bool bWorking)
    {
    }
}