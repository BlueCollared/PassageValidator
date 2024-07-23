using EtGate.Domain;

namespace Domain.Peripherals.Qr
{
    public interface IQrReaderStatus : IDeviceStatus<QrReaderStatus> { }
    public record QrReaderStatus (bool bConnected, string firmwareVersion, bool bScanning) : ModuleStatus
    {
        //[JsonInclude]
        //public readonly bool bConnected;

        //[JsonInclude]
        //public readonly string firmwareVersion;

        //[JsonInclude]
        //public readonly bool bScanning;

        //public QrReaderStatus(bool bConnected, string firmwareVersion, bool bScanning)
        //{
        //    this.bConnected = bConnected;
        //    this.firmwareVersion = firmwareVersion;
        //    this.bScanning = bScanning;
        //}

        //public bool IsAvailable => bConnected;

        public bool IsAvailable => bConnected;
    };
}