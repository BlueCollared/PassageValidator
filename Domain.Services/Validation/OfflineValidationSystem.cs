using Domain.Peripherals.Qr;
using Equipment.Core.Message;
using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services.Validation
{
    // TODO: listen to the manifest data, maintain that data, and use it for making decisions
    // but it will not decide that the data has gone stale. It depends upon the notification from status manager for it.
    public class OfflineValidationSystem : IValidate
    {
        private IDeviceStatusPublisher<OfflineValidationSystemStatus> statusMgr;

        public OfflineValidationSystem(IDeviceStatusPublisher<OfflineValidationSystemStatus> statusMgr)
        {
            this.statusMgr = statusMgr;
            //'statusMgr.Subscribe(x => Debug.WriteLine($"Offline {x}"));
        }

        //public IObservable<OfflineValidationSystemStatus> StatusStream
          //  => statusMgr.statusObservable;

        //public bool IsWorking => statusMgr.IsWorking;
        
        //private readonly IDeviceStatus<OfflineValidationSystemStatus> statusMgr;

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}
