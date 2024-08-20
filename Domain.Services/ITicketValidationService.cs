using Domain.Peripherals.Gate;

namespace Domain.Services
{
    public interface ITicketValidationService
    {
        bool ValidateQrCode(Ticket ticket);
        void UpdatePassengerStatus(bool hasCrossedZoneB);
        IPassageController PassageController { get; }
    }
}