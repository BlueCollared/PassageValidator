namespace EtGate.Domain.ValidationSystem
{
    public record OnlineValidationSystemStatus (bool bConnected) : ModuleStatus
    {        
        public bool IsAvailable => bConnected;
    }
}
