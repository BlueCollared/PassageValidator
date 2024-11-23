using Domain.Peripherals.Qr;
using Equipment.Core.Message;
using EtGate.Domain.Services.Qr;

namespace EtGate.QrReader.Proxy
{
    public class QrReaderDeviceControllerProxy : IQrReaderController//QrReaderDeviceControllerBase//, IQrReaderStatus
    {
        public const string pipeName = "QrSimul";
        private readonly IDeviceStatusPublisher<QrReaderStatus> messagePublisher;
        private readonly IMessagePublisher<QrCodeInfo> qrCodeInfo;
        SimulatorListener listener;//= new SimulatorListener(pipeName);
        //ReqClient client = new ReqClient(pipeName, new List<Type> { typeof(QrReaderStatus), typeof(QrCodeInfo) }, new List<Type> { typeof(StartReq), typeof(StartDetectingReq) });
        public QrReaderDeviceControllerProxy(
            IDeviceStatusPublisher<QrReaderStatus> messagePublisher,
            IMessagePublisher<QrCodeInfo> qrCodeInfo
            //IMessagePublisher<QrReaderStaticData> qrCodeStaticData
            )//:base(qrCodeInfo, qrCodeStaticData)
        {
            //AsyncCaller svr = new AsyncCaller("QrReader");
            listener = new SimulatorListener(pipeName, this);
            this.messagePublisher = messagePublisher;
            this.qrCodeInfo = qrCodeInfo;
        }

        internal bool StartAnswer;
        public bool Start()
        {
            return StartAnswer;
        }

        internal bool StartDetectingAnswer;
        public bool StartDetecting()
        {
            return StartDetectingAnswer;
        }

        public void Stop()
        {
            return;
        }

        public void StopDetecting()
        {
            return;
        }

        internal void Notify(QrReaderStatus x)
        {
            messagePublisher.Publish(x);
        }

        internal void Notify(QrCodeInfo x)
        {
            qrCodeInfo.Publish(x);
        }
    }
}