using Domain;
using Domain.Services;
using EtGate.Domain.Services.Gate;

namespace EtGate.Devices.Interfaces.Validation
{
    // 22010-2024 TODO: `PassageMgr` is  HORRIBLE MISFIT here. still giving some time before eliminating this code
    public class TicketValidationService : ITicketValidationService
    {
        //private readonly IPassageController _flapController;
        private PassengerStatus _passengerStatus;
        //private GateStatus _gateStatus;

        //public IPassageController PassageController => _flapController;
        //public IPassageController PassageMgr { get; private set; }

        public TicketValidationService(IGateInServiceController passageMgr)
        {
          //  PassageMgr = passageMgr;
            _passengerStatus = new PassengerStatus();
            //_gateStatus = new GateStatus { IsInService = false, IsQrReaderWorking = false, IsFlapControllerWorking = false };
        }

        public bool ValidateQrCode(Ticket ticket)
        {
            if (ticket.IsValid
                //&& _gateStatus.IsInService 
                && !_passengerStatus.FlapOpened)
            {
            //    PassageMgr.Authorize(1, bEntry:true); // TODO: correct it
                _passengerStatus.FlapOpened = true;
                return true;
            }
            return false;
        }

        public void UpdatePassengerStatus(bool hasCrossedZoneB)
        {
            _passengerStatus.HasCrossedZoneB = hasCrossedZoneB;
            if (hasCrossedZoneB)
            {
                _passengerStatus = new PassengerStatus();
            }
        }

        //public void UpdateGateStatus(bool isQrReaderWorking, bool isFlapControllerWorking)
        //{
        //    _gateStatus.IsQrReaderWorking = isQrReaderWorking;
        //    _gateStatus.IsFlapControllerWorking = isFlapControllerWorking;
        //    _gateStatus.IsInService = isQrReaderWorking && isFlapControllerWorking;
        //}

        //public GateStatus GetGateStatus()
        //{
        //    return _gateStatus;
        //}

    }
}
