using Domain.Peripherals.Qr;
using EtGate.Devices.Interfaces;
using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services.Validation
{
    public class OfflineValidationSubsytem : OfflineValidationBase
    {
        public bool IsWorking => throw new NotImplementedException();

        public IObservable<ModuleStatus> statusObservable => throw new NotImplementedException();

        public override QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }

        public override bool Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public IObservable<OfflineValidationSystemStatus> status;
    }
}
