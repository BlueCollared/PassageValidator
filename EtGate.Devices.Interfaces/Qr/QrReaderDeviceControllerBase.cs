using Domain.Peripherals.Qr;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Devices.Interfaces.Qr
{
    public abstract class QrReaderDeviceControllerBase : StatusStreamBase<QrReaderStatus>, IQrReaderControllerEx
    {
        readonly protected Subject<QrCodeInfo> qrCodeInfoSubject = new();
        public IObservable<QrCodeInfo> qrCodeInfoObservable => qrCodeInfoSubject.AsObservable();
        
        readonly protected ReplaySubject<QrReaderStaticData> qrCodeStatic = new();
        public IObservable<QrReaderStaticData> qrReaderStaticDataObservable => qrCodeStatic.AsObservable();

        public abstract bool Start();
        public abstract void Stop();

        public abstract bool StartDetecting();
        public abstract void StopDetecting();

        abstract public void Reboot();
    }
}