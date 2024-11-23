using Domain.Peripherals.Qr;
using Equipment.Core.Message;
using EtGate.Domain.ValidationSystem;
using System.Diagnostics;
using System.Reactive.Linq;

namespace EtGate.Domain.Services.Validation
{
    // TODO: Listen to the current site / and other such configuration so as to make pre-checks on the ticket
    public class ValidationMgr //: IValidationMgr
    {
        // better to be direct i.e.OfflineValidationSystem, OnlineValidationSystem
        //public List<IValidate> validationSubSystems = new(); 
        IValidate offline;
        IValidate online;
        private readonly DeviceStatusSubscriber<OnlineValidationSystemStatus> onlineDeviceStatus;
        private readonly DeviceStatusSubscriber<OfflineValidationSystemStatus> offlineDeviceStatus;

        public ValidationMgr(
            IValidate online,
            DeviceStatusSubscriber<OnlineValidationSystemStatus> onlineDeviceStatus,
            IValidate offline,
            DeviceStatusSubscriber<OfflineValidationSystemStatus> offlineDeviceStatus            
            )
        {
            this.offline = offline;
            this.online = online;
            this.onlineDeviceStatus = onlineDeviceStatus;
            this.offlineDeviceStatus = offlineDeviceStatus;
            StatusStream.Subscribe(x =>
            {
                status = x;
                Debug.WriteLine($"Validation {x}"); }
            );
        }

        public IObservable<ValidationSystemStatus> StatusStream
            => Observable.CombineLatest(
            onlineDeviceStatus.Messages,
            offlineDeviceStatus.Messages,
            (on, off) => new ValidationSystemStatus(on, off)
            );

        public ValidationSystemStatus status;
        public bool IsWorking => status?.IsAvailable ?? false;

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            if (onlineDeviceStatus.curStatus.IsAvailable)
            {
                var result = online.Validate(qrCode);
                if (result != QrCodeValidationResult.CallNotMade)
                    return result;
            }
            else if (offlineDeviceStatus.curStatus.IsAvailable)
            {
                var result = offline.Validate(qrCode);
                if (result != QrCodeValidationResult.CallNotMade)
                    return result;
            }
            return QrCodeValidationResult.CallNotMade;
        }
    }
}