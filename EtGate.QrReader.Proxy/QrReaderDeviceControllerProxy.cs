using Domain.Peripherals.Qr;

namespace EtGate.QrReader.Proxy
{
    public class QrReaderDeviceControllerProxy : QrReaderDeviceControllerBase
    {
        public const string pipeName = "QrSimul";
        SimulatorListener listener;//= new SimulatorListener(pipeName);
        //ReqClient client = new ReqClient(pipeName, new List<Type> { typeof(QrReaderStatus), typeof(QrCodeInfo) }, new List<Type> { typeof(StartReq), typeof(StartDetectingReq) });
        public QrReaderDeviceControllerProxy()
        {
            //AsyncCaller svr = new AsyncCaller("QrReader");
            listener = new SimulatorListener(pipeName, this);
        }

        internal bool StartAnswer;
        public override bool Start()
        {
            return StartAnswer;
        }

        internal bool StartDetectingAnswer;
        public override bool StartDetecting()
        {
            return StartDetectingAnswer;
        }

        public override void Stop()
        {
            return;
        }

        public override void StopDetecting()
        {
            return;
        }

        internal void Notify(QrReaderStatus x)
        {
            statusSubject.OnNext(x);
        }

        internal void Notify(QrCodeInfo x)
        {
            qrCodeInfoSubject.OnNext(x);
        }
    }
}