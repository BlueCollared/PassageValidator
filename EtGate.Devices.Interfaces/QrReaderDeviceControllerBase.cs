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
        protected ReplaySubject<QrReaderStatus> statusSubject = new();

        public IObservable<QrReaderStatus> statusObservable => statusSubject.AsObservable()
            .ObserveOn(SynchronizationContext.Current);

        public IObservable<QrCodeInfo> qrCodeInfoObservable => qrCodeInfoSubject.AsObservable();

        public abstract bool Start();
        public abstract void Stop();

        public abstract bool StartDetecting();        
        public abstract void StopDetecting();

        QrReaderStatus CurStatus { get; set; } = null;

        public bool IsWorking => CurStatus == null ? false : CurStatus.bConnected;

        IDisposable qrRdrStatusSubscription;

        protected QrReaderDeviceControllerBase()
        {
            qrRdrStatusSubscription = statusObservable.Subscribe(status => CurStatus = status);
        }
    }
}