using EtGate.Domain.ValidationSystem;
using System.Text.Json.Serialization;

namespace Domain.Peripherals.Qr
{
    public interface IQrReaderStatus : IDeviceStatus<QrReaderStatus> { }
    public class QrReaderStatus : ModuleStatus
    {
        [JsonInclude]
        public readonly bool bConnected;

        [JsonInclude]
        public readonly string firmwareVersion;

        [JsonInclude]
        public readonly bool bScanning;

        public QrReaderStatus(bool bConnected, string firmwareVersion, bool bScanning)
        {
            this.bConnected = bConnected;
            this.firmwareVersion = firmwareVersion;
            this.bScanning = bScanning;
        }

        public override bool IsAvailable => bConnected;
    }
}