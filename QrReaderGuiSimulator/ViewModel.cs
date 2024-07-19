using Domain.Peripherals.Qr;
using EtGate.QrReader.Proxy;

namespace QrReaderGuiSimulator
{
    internal class ViewModel
    {
        internal StartDetectingResp StartDetectingResp;
        internal StartResp StartResp;

        public QrReaderStatus QrReaderStatus { get; internal set; }
    }
}
