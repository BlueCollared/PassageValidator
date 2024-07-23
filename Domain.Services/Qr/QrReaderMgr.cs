using Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Qr
{
    // it is getting little difficult to decide whether to make it implement IQrReaderStatus, IQrInfoStatus. Ideally we keep it so simple that there is not need of it
    // the 
    public class QrReaderMgr //: IQrReaderStatus, IQrInfoStatus
    {
        private readonly IQrReader qrRdrInfo;
        private readonly IQrReaderStatus statusMgr;

        //List<IQrReader> qrRdrs = new List<IQrReader>();
        public QrReaderMgr(
            //QrMgrConfig config,
            IQrReader qrRdrInfo,
            IQrReaderStatus statusMgr
            )
        {
            this.statusMgr = statusMgr;
            this.qrRdrInfo = qrRdrInfo;

            //qrRdrStatusSubscription =
            //((IObservable<QrReaderStatus>)qrRdr)
            //rdr.qrReaderStatusObservable
            //    .ObserveOn(SynchronizationContext.Current)
            //    .Subscribe(x => {
            //        // TODO: raise event (pump into message bus)
            //    });
            //rdr.StartListeningStatus(this);
            // TODO: create IQrReader's using {qrFactory, config} and push them to qrRdrs.
            // `config` would also contain the `id` of that reader. This `id` would be bounced back in the 
            // event that is raised when the qr is detected
        }

        //readonly SynchronizationContext syncContextClient = SynchronizationContext.Current;

        public IObservable<QrReaderStatus> StatusStream
            => statusMgr.statusObservable
            //.ObserveOn(syncContextClient) // I remove it from here, otherwise the unit tests fail (but as per https://chatgpt.com/c/4df3ed5d-cada-4f6f-8355-e6a060c87aad would not fail in normal environment; only in the unit test environment)
            // also, it indicates leaky abstraction. I now move this to `IQrReaderStatus`
            ;

        public bool IsWorking => statusMgr.IsWorking;

        public IObservable<QrCodeInfo> QrCodeStream
            => qrRdrInfo.qrCodeInfoObservable
            //.ObserveOn(syncContextClient)
            ;

        //        public IObservable<QrReaderStatus> qrReaderStatusObservable => throw new NotImplementedException();

        //        public IObservable<QrCodeInfo> qrCodeInfoObservable => throw new NotImplementedException();


        public void StopDetecting()
        {
            qrRdrInfo.StopDetecting();
        }

        public bool StartDetecting()
        {
            qrRdrInfo.StartDetecting();
            return true;
        }
    }
}