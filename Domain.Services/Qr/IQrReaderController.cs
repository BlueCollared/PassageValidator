using Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Qr
{
    public interface IQrReaderController
    {
        //string id { get; } // typically would be "Entry"/"Exit"
        //IObservable<QrCodeInfo> qrCodeInfoObservable { get; }

        bool StartDetecting();
        void StopDetecting();
    }
}
