//using Domain.Peripherals.Qr;
//using Equipment.Core.Message;
//using EtGate.Domain.Services.Qr;

//namespace EtGate.Devices.Interfaces.Qr
//{
//    public abstract class QrReaderDeviceControllerBase : IQrReaderController
//    {
//        protected QrReaderDeviceControllerBase(IMessagePublisher<QrCodeInfo> qrCodeInfo,
//            IMessagePublisher<QrReaderStaticData> qrCodeStaticData)
//        {
//            this.qrCodeInfo = qrCodeInfo;
//            this.qrCodeStaticData = qrCodeStaticData;
//        }
////        readonly protected Subject<QrCodeInfo> qrCodeInfoSubject = new();
//  //      public IObservable<QrCodeInfo> qrCodeInfoObservable => qrCodeInfoSubject.AsObservable();
        
//        //readonly protected ReplaySubject<QrReaderStaticData> qrCodeStatic = new();
//        protected readonly IMessagePublisher<QrCodeInfo> qrCodeInfo;
//        protected readonly IMessagePublisher<QrReaderStaticData> qrCodeStaticData;

//        //public IObservable<QrReaderStaticData> qrReaderStaticDataObservable => qrCodeStatic.AsObservable();

//        public abstract bool Start();
//        public abstract void Stop();

//        public abstract bool StartDetecting();
//        public abstract void StopDetecting();        
//    }
//}