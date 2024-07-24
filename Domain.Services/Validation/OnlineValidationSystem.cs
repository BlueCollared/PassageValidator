using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services.Validation
{
    // Two thoughts: why this class implements `IValidate` thus leaving the room for a dummy implementation.
    // while we should be providing the dummy implementaion of the injected field?
    public class OnlineValidationSystem : IValidate
    {
        private readonly IOnlineValidationStatus statusMgr;
        private readonly IValidate worker;

        public IObservable<OnlineValidationSystemStatus> StatusStream
            => statusMgr.statusObservable;

        public bool IsWorking => statusMgr.IsWorking;

        public OnlineValidationSystem(IOnlineValidationStatus statusMgr, IValidate worker)
        {
            this.statusMgr = statusMgr;
            this.worker = worker;
        }

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            try
            {
                // TODO: make pre-checks on the qr, like it might have to be issued from the same station
                return worker.Validate(qrCode);
            }
            catch (Exception)
            {
                return QrCodeValidationResult.CallNotMade;
            }
        }
    }
}