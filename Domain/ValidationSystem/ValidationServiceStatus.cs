namespace EtGate.Domain.ValidationSystem
{
    abstract public class ValidataionSubSystemStatus
    {        
        public virtual bool IsAvailable { get; protected set; }
    }

    public class ValidationServiceStatus : ValidataionSubSystemStatus
    { }

    public class OfflineValidationSystemStatus : ValidataionSubSystemStatus
    {
        public DateTimeOffset validTill = DateTimeOffset.MinValue;

        public override bool IsAvailable => DateTimeOffset.Now > validTill;
    }
}