using EtGate.Domain;

namespace Domain.Peripherals.Qr
{
    public record QrReaderStatus (bool bConnected, string firmwareVersion, bool bScanning) : ModuleStatus
    {
        public bool IsAvailable => bConnected;

        public static QrReaderStatus GoodStatus => new QrReaderStatus(bConnected: true, "", bScanning: false);
    };
}