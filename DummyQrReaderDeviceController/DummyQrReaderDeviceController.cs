using EtGate.QrReader;

namespace DummyQrReaderDeviceController
{
    public class DummyQrReaderDeviceController : QrReaderDeviceControllerBase
    {
        Task tskStatusDetection;
        bool bStop = false;
        public override bool Start()
        {
            return true;
        }

        public override bool StartDetecting()
        {
            bStop = false;
            tskStatusDetection = Task.Run(()=>
                { 
                while(!bStop)
                {
                    qrReaderStatusSubject.OnNext(new Domain.Peripherals.Qr.QrReaderStatus(false, "", false));

                    Thread.Sleep(5000);

                    qrReaderStatusSubject.OnNext(new Domain.Peripherals.Qr.QrReaderStatus(true, "", false));

                        Thread.Sleep(5000);
                    }
            });
            return true;
        }

        public override bool Stop()
        {
            bStop = true;
            return true ;
        }

        public override void StopDetecting()
        {
            bStop = true;
        }
    }
}
