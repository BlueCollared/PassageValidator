using EtGate.Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Validation
{
    public interface IValidate
    {
        QrCodeValidationResult Validate(QrCodeInfo qrCode);
    }
}
