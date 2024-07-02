namespace Domain.Peripherals.Qr
{
    // I think we can have a single manager managing both readers. But very open to change this idea
    public class QrReaderMgr
    {
        public QrReaderMgr(IQrReaderFactory qrFactory, QrMgrConfig config)
        {

        }
    }
}