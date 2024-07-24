using EtGate.Domain;

namespace Domain.Peripherals.Qr
{
    public record QrReaderStatus (bool bConnected, string firmwareVersion, bool bScanning) : ModuleStatus
    {
        public bool IsAvailable => bConnected;

        public static QrReaderStatus AllGood => new QrReaderStatus(bConnected: true, "", bScanning: false);
        public static QrReaderStatus Disconnected => new QrReaderStatus(bConnected: false, "", bScanning: false);
    };
}