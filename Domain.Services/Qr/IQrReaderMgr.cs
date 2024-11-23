using Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Qr
{
    public interface IQrReaderMgr
    {
        //bool IsWorking { get; }
        IObservable<QrCodeInfo> QrCodeStream { get; }
        IObservable<QrReaderStatus> StatusStream { get; }

        bool StartDetecting();
        void StopDetecting();
    }
}