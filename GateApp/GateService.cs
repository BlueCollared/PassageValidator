using Domain;
using Domain.Services;

namespace GateApp
{
    // Application Layer
    public interface IGateService
    {
        bool ValidateQrCode(Ticket ticket);
        void SubscribeToZoneEvents();
        void UpdateGateStatus(bool isQrReaderWorking, bool isFlapControllerWorking);
        GateStatus GetGateStatus();
    }

    public class GateService : IGateService
    {
        private readonly ITicketValidationService _ticketValidationService;

        public GateService(ITicketValidationService ticketValidationService)
        {
            _ticketValidationService = ticketValidationService;
        }

        public bool ValidateQrCode(Ticket ticket)
        {
            return _ticketValidationService.ValidateQrCode(ticket);
        }

        public void SubscribeToZoneEvents()
        {
            _ticketValidationService.PassageController.ZoneEvents.Subscribe(zoneEvent =>
            {
                if (zoneEvent.Zone == "B")
                {
                    _ticketValidationService.UpdatePassengerStatus(true);
                }
            });
        }

        public void UpdateGateStatus(bool isQrReaderWorking, bool isFlapControllerWorking)
        {
            _ticketValidationService.UpdateGateStatus(isQrReaderWorking, isFlapControllerWorking);
        }

        public GateStatus GetGateStatus()
        {
            return _ticketValidationService.GetGateStatus();
        }
    }
}
