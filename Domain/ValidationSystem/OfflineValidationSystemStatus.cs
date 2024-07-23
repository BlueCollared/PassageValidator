namespace EtGate.Domain.ValidationSystem
{
    public record OfflineValidationSystemStatus (DateTimeOffset validTill) : ModuleStatus
    {
        //public override bool IsAvailable => DateTimeOffset.Now > validTill;

        public bool IsAvailable { get => DateTimeOffset.Now > validTill; }
    }
}
