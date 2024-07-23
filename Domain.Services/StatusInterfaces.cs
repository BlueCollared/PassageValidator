using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services
{
    public interface IQrReaderStatus : IDeviceStatus<QrReaderStatus>
    { }

    public interface IOnlineValidationStatus : IDeviceStatus<OnlineValidationSystemStatus>
    { }

    public interface IOfflineValidationStatus : IDeviceStatus<OfflineValidationSystemStatus>
    { }
}