using Domain.Peripherals.Qr;
using EtGate.QrReader.Proxy;

namespace QrReaderGuiSimulator
{
    public class ViewModel
    {
        internal StartDetectingResp StartDetectingResp;
        internal StartResp StartResp;

        public QrReaderStatus QrReaderStatus { get; internal set; }
    }
}
