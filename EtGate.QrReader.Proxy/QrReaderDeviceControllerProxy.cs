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
        public override bool Start()
        {
            //StartReq req = new();
            //var x = JsonConvert.SerializeObject(req);
            //var resp = client.RequestReplyAsync<StartReq, StartResp>(req, TimeSpan.FromSeconds(120));
            return true;
        }

        public override bool StartDetecting()
        {
            return true;
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
            qrReaderStatusSubject.OnNext(x);
        }
    }
}