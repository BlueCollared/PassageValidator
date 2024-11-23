using Equipment.Core;

namespace EtGate.Domain.ValidationSystem
{
    public record ValidationSystemStatus (OnlineValidationSystemStatus onlineStatus, OfflineValidationSystemStatus offlineStatus) : ModuleStatus
    {
        public override bool IsAvailable => (onlineStatus?.IsAvailable ?? false) || (offlineStatus?.IsAvailable ?? false);

        public static ValidationSystemStatus Default => new ValidationSystemStatus(OnlineValidationSystemStatus.Disconnected, OfflineValidationSystemStatus.Obsolete);        
    }
}