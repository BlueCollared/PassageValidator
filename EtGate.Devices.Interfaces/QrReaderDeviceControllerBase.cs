using Domain.Peripherals.Qr;
using EtGate.Devices.Interfaces;
using Peripherals;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.QrReader
{
    interface IQrReader : IPeripheral { }
    public abstract class QrReaderDeviceControllerBase : StatusStreamBase<QrReaderStatus>, IQrReader, Domain.Services.Qr.IQrReader
    {
        protected Subject<QrCodeInfo> qrCodeInfoSubject = new();

        public IObservable<QrCodeInfo> qrCodeInfoObservable => qrCodeInfoSubject.AsObservable();

        public abstract bool Start();
        public abstract void Stop();

        public abstract bool StartDetecting();        
        public abstract void StopDetecting();
    }
}