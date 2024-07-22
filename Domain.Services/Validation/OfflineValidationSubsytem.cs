using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services.Validation
{
    public class OfflineValidationSubsytem : IValidationSubSystem
    {
        public bool IsWorking => throw new NotImplementedException();

        public IObservable<ValidataionSubSystemStatus> statusObservable => throw new NotImplementedException();

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }

        public IObservable<OfflineValidationSystemStatus> status;
    }
}
