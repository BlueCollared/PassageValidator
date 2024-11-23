using Domain.Peripherals.Qr;
using Equipment.Core.Message;

namespace EtGate.Domain.Services.Qr
{
    // it is getting little difficult to decide whether to make it implement IQrReaderStatus, IQrInfoStatus. Ideally we keep it so simple that there is not need of it
    // the 
    public class QrReaderMgr : IQrReaderMgr
    //: IQrReaderStatus, IQrInfoStatus
    {
        private readonly IQrReaderController qrRdrInfo;
        private readonly DeviceStatusSubscriber<QrReaderStatus> statusMgr;
        private readonly IMessageSubscriber<QrCodeInfo> qrStream;

        //List<IQrReader> qrRdrs = new List<IQrReader>();
        public QrReaderMgr(
            //QrMgrConfig config,
            IQrReaderController qrRdrInfo,
            DeviceStatusSubscriber<QrReaderStatus> statusMgr,
            IMessageSubscriber<QrCodeInfo> qrStream
            )
        {
            this.statusMgr = statusMgr;
            this.qrStream = qrStream;
            this.qrRdrInfo = qrRdrInfo;

            StatusStream.Subscribe(x => { });

            // TODO: create IQrReader's using {qrFactory, config} and push them to qrRdrs.
            // `config` would also contain the `id` of that reader. This `id` would be bounced back in the 
            // event that is raised when the qr is detected
        }

        //readonly SynchronizationContext syncContextClient = SynchronizationContext.Current;

        public IObservable<QrReaderStatus> StatusStream
            => statusMgr.Messages
            //.ObserveOn(syncContextClient) // I remove it from here, otherwise the unit tests fail (but as per https://chatgpt.com/c/4df3ed5d-cada-4f6f-8355-e6a060c87aad would not fail in normal environment; only in the unit test environment)
            // also, it indicates leaky abstraction. I now move this to `IQrReaderStatus`
            ;

        //public bool IsWorking => statusMgr.IsWorking;

        public IObservable<QrCodeInfo> QrCodeStream
            => qrStream.Messages;
            //.ObserveOn(syncContextClient)
            

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