using EtGate.Domain.Services.Gate;

namespace Domain.Services
{
    public interface ITicketValidationService
    {
        bool ValidateQrCode(Ticket ticket);
        void UpdatePassengerStatus(bool hasCrossedZoneB);
        IPassageManager PassageMgr { get; }
    }
}