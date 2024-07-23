namespace EtGate.Domain.ValidationSystem
{
    public class ValidationSystemStatus : ModuleStatus
    {
        public OnlineValidationSystemStatus onlineStatus;
        public OfflineValidationSystemStatus offlineStatus;

        public bool IsAvailable => onlineStatus.IsAvailable || offlineStatus.IsAvailable;
    }
}
