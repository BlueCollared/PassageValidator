using Domain.Peripherals.Qr;
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
                    statusSubject.OnNext(new QrReaderStatus(false, "", false));

                    Thread.Sleep(5000);

                    statusSubject.OnNext(new QrReaderStatus(true, "", false));

                        Thread.Sleep(5000);
                    }
            });
            return true;
        }

        public override void Stop()
        {
            bStop = true;            
        }

        public override void StopDetecting()
        {
            bStop = true;
        }
    }
}
