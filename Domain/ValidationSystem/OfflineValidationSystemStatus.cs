namespace EtGate.Domain.ValidationSystem
{
    public record OfflineValidationSystemStatus (
        DateTimeOffset validTill, 
        DateTimeOffset lastFetched) : ModuleStatus
    {
        public bool IsAvailable { get => DateTimeOffset.Now > validTill; }
    }
}
