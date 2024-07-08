using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using QrReader;

namespace GateApp
{
    public class Application
    {
        ModeManager modeManager;
        QrReaderMgr qrRdrMgr;
        public Application()
        {
            qrRdrMgr = new QrReaderMgr(new QrReader.QrReader());            
        }
    }
}
