using Domain;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;

namespace GateApp
{
    public class Application
    {
        ModeManager modeManager;
        QrReaderMgr qrRdrMgr;
        DomainEvtMgr domainEvtMgr;

        public Application()
        {
            //// 1. Set up the system
            //qrRdrMgr = new QrReaderMgr(new QrReader.QrReader());
            //modeManager = new ModeManager(qrRdrMgr);
            //domainEvtMgr = new DomainEvtMgr(new DomainEvtDbPersister(), qrRdrMgr);

            // 2. Start the system. Two steps because we don't want to loose any event
            //qrRdrMgr.Start();
        }
    }
}