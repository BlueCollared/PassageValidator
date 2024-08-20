using Domain.Peripherals.Qr;
using EtGate.Devices.Interfaces.Validation;

namespace EtGate.Domain.Services.Validation
{
    public class OfflineValidationSubsytem : OfflineValidationBase
    {
        public override bool Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}
