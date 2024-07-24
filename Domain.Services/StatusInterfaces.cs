using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services
{
    //public abstract class IQrReaderStatus : IDeviceStatus<QrReaderStatus>
    //{ }

    public abstract class IOnlineValidationStatus : IDeviceStatus<OnlineValidationSystemStatus>
    { }

    public abstract class IOfflineValidationStatus : IDeviceStatus<OfflineValidationSystemStatus>
    { }

    public abstract class IValidationStatus : IDeviceStatus<ValidationSystemStatus>
    { }
}