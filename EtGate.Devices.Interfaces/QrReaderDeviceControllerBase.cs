using Domain.Peripherals.Qr;
using Peripherals;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.QrReader
{
    interface IQrReader : IPeripheral { }
    public abstract class QrReaderDeviceControllerBase : IQrReader, IQrReaderStatus, IQrInfoStatus
    {
        protected Subject<QrCodeInfo> qrCodeInfoSubject = new();
        protected ReplaySubject<QrReaderStatus> qrReaderStatusSubject = new();

        public IObservable<QrReaderStatus> qrReaderStatusObservable => qrReaderStatusSubject.AsObservable()
            .ObserveOn(SynchronizationContext.Current);

        public IObservable<QrCodeInfo> qrCodeInfoObservable => qrCodeInfoSubject.AsObservable();

        public abstract bool Start();
        public abstract bool StartDetecting();
        public abstract bool Stop();
        public abstract void StopDetecting();
    }
}