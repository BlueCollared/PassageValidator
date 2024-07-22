using Domain.Peripherals.Qr;
using EtGate.Domain;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using Peripherals;

namespace EtGate.Devices.Interfaces
{
    abstract public class OfflineValidationBase : StatusStreamBase<OfflineValidationSystemStatus>, IPeripheral, IValidationSubSystem
    {
        public abstract bool Start();
        public abstract void Stop();
        public abstract QrCodeValidationResult Validate(QrCodeInfo qrCode);
    }
}
