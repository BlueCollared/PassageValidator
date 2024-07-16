using Domain.Peripherals.Passage;

namespace Domain.Services
{
    public interface ITicketValidationService
    {
        bool ValidateQrCode(Ticket ticket);
        void UpdatePassengerStatus(bool hasCrossedZoneB);
        void UpdateGateStatus(bool isQrReaderWorking, bool isFlapControllerWorking);
        GateStatus GetGateStatus();
        IPassageController PassageController { get; }

    }
}