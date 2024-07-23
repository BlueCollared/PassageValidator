using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services.Validation
{
    // TODO: listen to the manifest data, maintain that data, and use it for making decisions
    public class OfflineValidationSystem : IValidate
    {
        public IObservable<OfflineValidationSystemStatus> StatusStream
            => statusMgr.statusObservable;

        public bool IsWorking => statusMgr.IsWorking;
        
        private readonly IOfflineValidationStatus statusMgr;

        public OfflineValidationSystem(IOfflineValidationStatus statusMgr)
        {
            this.statusMgr = statusMgr;
        }

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}
