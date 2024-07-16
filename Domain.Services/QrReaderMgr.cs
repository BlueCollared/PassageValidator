using System.Reactive.Linq;

namespace Domain.Peripherals.Qr
{
    public class QrReaderMgr
    {
        private readonly IQrReader rdr;
        IQrReader qrRdr; // if we would have multiple readers, we would use a List<IQrReader>. Not unnecessarily complicating.
        IDisposable qrRdrStatusSubscription;
        //List<IQrReader> qrRdrs = new List<IQrReader>();
        public QrReaderMgr(
            //IQrReaderFactory qrFactory, // I don't see any benefit in using IQrReaderFactory because we are creating IQrReader in the constructor itself
            //QrMgrConfig config,
            IQrReader rdr
            //Action<QrCodeInfo> qrAppeared,
//            IScheduler scheduler// TODO: see if we need it here OR at its client level OR not at all
            )
        {
            this.rdr = rdr;
            qrRdrStatusSubscription =
            //((IObservable<QrReaderStatus>)qrRdr)
            qrRdr.qrReaderStatusObservable
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x => {
                    // TODO: raise event (pump into message bus)
                });
            //rdr.StartListeningStatus(this);
            // TODO: create IQrReader's using {qrFactory, config} and push them to qrRdrs.
            // `config` would also contain the `id` of that reader. This `id` would be bounced back in the 
            // event that is raised when the qr is detected
        }

        readonly SynchronizationContext syncContextClient = SynchronizationContext.Current;
        
        public IObservable<QrReaderStatus> StatusStream
            => qrRdr.qrReaderStatusObservable
            .ObserveOn(syncContextClient);

        public IObservable<QrCodeInfo> QrCodeStream
            => qrRdr.qrCodeInfoObservable
            .ObserveOn(syncContextClient);

        public void StartDetecting()
        {
            rdr.StartDetecting();
        }

        public void StopDetecting()
        {
            rdr.StopDetecting();
        }
    }
}