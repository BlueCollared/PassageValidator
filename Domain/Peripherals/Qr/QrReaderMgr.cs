namespace Domain.Peripherals.Qr
{
    // I think we can have a single manager instance managing both readers. But very open to change this idea
    public class QrReaderMgr
    {
        List<IQrReader> qrRdrs = new List<IQrReader>();
        public QrReaderMgr(
            IQrReaderFactory qrFactory, 
            QrMgrConfig config, 
            Action<QrCodeInfo> qrAppeared
            //, IScheduler scheduler  TODO: see if we need it here OR at its client level OR not at all
            )
        {
            // TODO: create IQrReader's using {qrFactory, config} and push them to qrRdrs.
            // `config` would also contain the `id` of that reader. This `id` would be bounced back in the 
            // event that is raised when the qr is detected
        }
    }
}