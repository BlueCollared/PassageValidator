using Domain.Peripherals.Qr;
using Equipment.Core.Message;
using EtGate.Domain.ValidationSystem;
using System.Diagnostics;

namespace EtGate.Domain.Services.Validation
{
    // Two thoughts: why this class implements `IValidate` thus leaving the room for a dummy implementation.
    // while we should be providing the dummy implementaion of the injected field?
    public class OnlineValidationSystem : IValidate
    {
        public OnlineValidationSystem(IDeviceStatusPublisher<OnlineValidationSystemStatus> statusMgr)
        {
            this.statusMgr = statusMgr;
            //StatusStream.Subscribe(x => Debug.WriteLine($"Online {x}"));
        }

        //public IObservable<OnlineValidationSystemStatus> StatusStream
          //  => statusMgr.mes

        //public bool IsWorking => statusMgr.IsWorking;

        private readonly IDeviceStatusPublisher<OnlineValidationSystemStatus> statusMgr;

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}