using Equipment.Core;

namespace EtGate.Domain.Peripherals.Qr
{
    public record QrReaderStatus (bool bConnected, bool bScanning) //: ModuleStatus
    {        
        public bool IsAvailable => bConnected;

        public static QrReaderStatus AllGood => new QrReaderStatus(bConnected: true, bScanning: false);
        public static QrReaderStatus Disconnected => new QrReaderStatus(bConnected: false, bScanning: false);

        //public ModuleStatus defStatus => Disconnected;
    };

    public record QrReaderStaticData(string firmwareInfo);
}