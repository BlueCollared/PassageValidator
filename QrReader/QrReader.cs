using Domain.Peripherals.Qr;

namespace QrReader
{
    public class QrReader : IQrReader
    {
        public string id => throw new NotImplementedException();

        public IObservable<QrCodeInfo> qrCodeInfoObservable => throw new NotImplementedException();

        public IObservable<QrReaderStatus> qrReaderStatusObservable => throw new NotImplementedException();

        public bool Init()
        {
            throw new NotImplementedException();
        }

        public bool StartDetecting()
        {
            throw new NotImplementedException();
        }

        public void StartListeningStatus(IObserver<QrReaderStatus> qrRdrStatus)
        {
            throw new NotImplementedException();
        }

        public bool Stop()
        {
            throw new NotImplementedException();
        }

        public void StopDetecting()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<QrReaderStatus> observer)
        {
            throw new NotImplementedException();
        }
    }
}
