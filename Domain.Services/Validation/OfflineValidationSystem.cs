using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;
using System.Diagnostics;

namespace EtGate.Domain.Services.Validation
{
    // TODO: listen to the manifest data, maintain that data, and use it for making decisions
    // but it will not decide that the data has gone stale. It depends upon the notification from status manager for it.
    public class OfflineValidationSystem : IValidate
    {
        public OfflineValidationSystem(IDeviceStatus<OfflineValidationSystemStatus> statusMgr)
        {
            this.statusMgr = statusMgr;
            StatusStream.Subscribe(x => Debug.WriteLine($"Offline {x}"));
        }

        public IObservable<OfflineValidationSystemStatus> StatusStream
            => statusMgr.statusObservable;

        public bool IsWorking => statusMgr.IsWorking;
        
        private readonly IDeviceStatus<OfflineValidationSystemStatus> statusMgr;

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}
