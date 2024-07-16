using Domain;
using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services;
using Domain.Services.Modes;
using GateApp;
using QrReader;

namespace App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //GateApp.Application app = new();

            QrReaderDeviceController qrRdr = new QrReaderDeviceController();
            QrReaderMgr qrRdrMgr = new QrReaderMgr(qrRdr, qrRdr);
            ValidationMgr validationMgr = new ValidationMgr();

            ModeManager modeManager = new ModeManager(qrRdrMgr, validationMgr, new PassageMgr());
            PassageMgr passageMgr = new PassageMgr();
            
            qrRdr.Start();

            //domainEvtMgr = new DomainEvtMgr(new DomainEvtDbPersister(), qrRdrMgr);

            Console.ReadLine();
        }
    }
}
