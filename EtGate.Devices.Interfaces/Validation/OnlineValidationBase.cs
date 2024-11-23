using Domain.Peripherals.Qr;
using EtGate.Domain;
using Peripherals;

namespace EtGate.Devices.Interfaces.Validation
{
    abstract public class OnlineValidationBase : IPeripheral
    {
        public abstract bool Start();
        public abstract void Stop();
        public abstract QrCodeValidationResult Validate(QrCodeInfo qrCode);
    }
}
