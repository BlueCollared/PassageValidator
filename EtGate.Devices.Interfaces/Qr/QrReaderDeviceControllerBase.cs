using Domain.Peripherals.Qr;
using EtGate.Domain.Services.Qr;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Devices.Interfaces.Qr
{
    public abstract class QrReaderDeviceControllerBase : StatusStreamBase<QrReaderStatus>, IQrReaderController
    {
        readonly protected Subject<QrCodeInfo> qrCodeInfoSubject = new();
        public IObservable<QrCodeInfo> qrCodeInfoObservable => qrCodeInfoSubject.AsObservable();
        
        readonly protected ReplaySubject<QrReaderStaticData> qrCodeStatic = new();
        public IObservable<QrReaderStaticData> qrReaderStaticDataObservable => qrCodeStatic.AsObservable();

        public abstract bool Start();
        public abstract void Stop();

        public abstract bool StartDetecting();
        public abstract void StopDetecting();        
    }
}