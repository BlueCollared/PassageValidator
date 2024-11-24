using EtGate.Domain;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.Services.Validation;
using Peripherals;

namespace EtGate.Devices.Interfaces.Validation
{
    abstract public class OfflineValidationBase : IPeripheral, IValidate
    {
        public abstract bool Start();
        public abstract void Stop();
        public abstract QrCodeValidationResult Validate(QrCodeInfo qrCode);
    }
}
