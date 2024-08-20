using Domain.Peripherals.Qr;
using EtGate.Devices.Interfaces;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Devices.Interfaces.Qr
{
    public abstract class QrReaderDeviceControllerBase : StatusStreamBase<QrReaderStatus>, IQrReaderControllerEx
    {
        protected Subject<QrCodeInfo> qrCodeInfoSubject = new();
        public IObservable<QrCodeInfo> qrCodeInfoObservable => qrCodeInfoSubject.AsObservable();

        public IObservable<QrReaderStaticData> qrReaderStaticDataObservable => qrCodeStatic.AsObservable();
        protected ReplaySubject<QrReaderStaticData> qrCodeStatic = new();

        public abstract bool Start();
        public abstract void Stop();

        public abstract bool StartDetecting();
        public abstract void StopDetecting();

        abstract public void Reboot();
    }
}