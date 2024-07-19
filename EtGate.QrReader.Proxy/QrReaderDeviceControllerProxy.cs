using EtGate.Proxy;

namespace EtGate.QrReader.Proxy
{
    public class QrReaderDeviceControllerProxy : QrReaderDeviceControllerBase
    {
        public QrReaderDeviceControllerProxy()
        {
            AsyncCaller svr = new AsyncCaller("QrReader");
        }
        public override bool Start()
        {
            return true;
        }

        public override bool StartDetecting()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override void StopDetecting()
        {
            throw new NotImplementedException();
        }
    }
}