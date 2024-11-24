using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.Services.Qr;
using EtGate.QrReader.Proxy;

namespace App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //GateApp.Application app = new();

            DeviceStatusBus<QrReaderStatus> status = new();
            EventBus<QrCodeInfo> qrInfo = new();
            QrReaderDeviceControllerProxy qrRdr = new QrReaderDeviceControllerProxy(status, qrInfo);
            QrReaderMgr qrRdrMgr = new QrReaderMgr(qrRdr, status, qrInfo);
            //ValidationMgr validationMgr = new ValidationMgr();

            //ModeManager modeManager = new ModeManager(qrRdrMgr, validationMgr, new PassageMgr());
            //PassageMgr passageMgr = new PassageMgr();
            
            qrRdr.Start();

            //domainEvtMgr = new DomainEvtMgr(new DomainEvtDbPersister(), qrRdrMgr);

            Console.ReadLine();
        }
    }
}
