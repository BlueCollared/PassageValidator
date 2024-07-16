using Domain.Peripherals.Qr;
using Peripherals;

namespace QrReader
{
    interface IQrReader : IPeripheral { }

    public class QrReaderDeviceController : IQrReader, IQrReaderStatus, IQrInfoStatus
    {
        public string id => throw new NotImplementedException();

        public IObservable<QrCodeInfo> qrCodeInfoObservable => throw new NotImplementedException();

        public IObservable<QrReaderStatus> qrReaderStatusObservable => throw new NotImplementedException();

        public bool Start()
        {
            throw new NotImplementedException();
        }

        public bool Stop()
        {
            throw new NotImplementedException();
        }

        public bool StartDetecting()
        {
            throw new NotImplementedException();
        }

        public void StopDetecting()
        {
            throw new NotImplementedException();
        }
    }
}