using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;
using System.Diagnostics;

namespace EtGate.Domain.Services.Validation
{
    // Two thoughts: why this class implements `IValidate` thus leaving the room for a dummy implementation.
    // while we should be providing the dummy implementaion of the injected field?
    public class OnlineValidationSystem : IValidate
    {
        public OnlineValidationSystem(IDeviceStatus<OnlineValidationSystemStatus> statusMgr)
        {
            this.statusMgr = statusMgr;
            StatusStream.Subscribe(x => Debug.WriteLine($"Online {x}"));
        }

        public IObservable<OnlineValidationSystemStatus> StatusStream
            => statusMgr.statusObservable;

        public bool IsWorking => statusMgr.IsWorking;

        private readonly IDeviceStatus<OnlineValidationSystemStatus> statusMgr;

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}