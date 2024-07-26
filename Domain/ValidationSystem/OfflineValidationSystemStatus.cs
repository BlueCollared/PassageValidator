namespace EtGate.Domain.ValidationSystem
{
    public record OfflineValidationSystemStatus (
        DateTimeOffset validTill, 
        DateTimeOffset lastFetched) : ModuleStatus
    {
        public override bool IsAvailable { get => DateTimeOffset.Now > validTill; }

        // TODO: RED flag
        public static OfflineValidationSystemStatus AllGood => new OfflineValidationSystemStatus(validTill: DateTimeOffset.MaxValue, lastFetched: DateTimeOffset.Now);

        // TODO: RED flag
        public static OfflineValidationSystemStatus Obsolete => new OfflineValidationSystemStatus(validTill: DateTimeOffset.MinValue, lastFetched: DateTimeOffset.Now);
    }
}
