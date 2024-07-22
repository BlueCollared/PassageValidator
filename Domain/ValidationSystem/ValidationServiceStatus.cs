namespace EtGate.Domain.ValidationSystem
{
    abstract public class ModuleStatus
    {        
        public virtual bool IsAvailable { get; protected set; }
    }

    public class ValidationServiceStatus : ModuleStatus
    { }

    public class OfflineValidationSystemStatus : ModuleStatus
    {
        public DateTimeOffset validTill = DateTimeOffset.MinValue;

        public override bool IsAvailable => DateTimeOffset.Now > validTill;
    }
}