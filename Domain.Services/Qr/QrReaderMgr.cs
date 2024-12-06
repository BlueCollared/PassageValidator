using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Qr
{
    // it is getting little difficult to decide whether to make it implement IQrReaderStatus, IQrInfoStatus. Ideally we keep it so simple that there is not need of it
    // the 
    public class QrReaderMgr : IQrReaderMgr
    //: IQrReaderStatus, IQrInfoStatus
    {
        private readonly IQrReaderController qrEntry;
        private readonly IQrReaderController qrExit;
        private readonly DeviceStatusSubscriber<QrReaderStatus> statusMgr;

        //List<IQrReader> qrRdrs = new List<IQrReader>();
        public QrReaderMgr(
            IQrReaderController qrEntry,
            DeviceStatusSubscriber<QrReaderStatus> statusMgr
            )
        {
            this.statusMgr = statusMgr;
            this.qrEntry = qrEntry;

            //StatusStream.Subscribe(x => { });

            // TODO: create IQrReader's using {qrFactory, config} and push them to qrRdrs.
            // `config` would also contain the `id` of that reader. This `id` would be bounced back in the 
            // event that is raised when the qr is detected
        }

        public async Task<(string ReaderMnemonic, QrCodeInfo QrCodeInfo)> StartDetecting(string qrs, CancellationToken inServiceCycleToken)
        {
            try
            {
                using var internalCts = new CancellationTokenSource();

                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(inServiceCycleToken, internalCts.Token);


                if (qrs == IQrReaderMgr.Both)
                {
                    var entryTask = DetectAsync(IQrReaderMgr.Entry, qrEntry, linkedCts.Token);
                    var exitTask = DetectAsync(IQrReaderMgr.Exit, qrExit, linkedCts.Token);

                    var completedTask = await Task.WhenAny(entryTask, exitTask);

                    // Cancel the other detection
                    internalCts.Cancel();

                    // Await both tasks to ensure they finish (even if cancelled)
                    var results = await Task.WhenAll(entryTask, exitTask);

                    // Extract the result of the successfully completed task (if any)
                    var detectionResult = results.FirstOrDefault(r => r.QrCodeInfo != null); // the default value is (null, null)

                    if (detectionResult.QrCodeInfo == null)
                    {
                        throw new OperationCanceledException("Polling was stopped without detecting any QR code.");
                    }

                    return detectionResult;
                }
                else if (qrs == IQrReaderMgr.Entry)
                    return await DetectAsync(IQrReaderMgr.Entry, qrEntry, inServiceCycleToken);
                else if (qrs == IQrReaderMgr.Exit)
                    return await DetectAsync(IQrReaderMgr.Exit, qrExit, inServiceCycleToken);
            }
            catch (OperationCanceledException) when (inServiceCycleToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("In-service cycle was cancelled.", inServiceCycleToken);
            }
            throw new Exception();
        }

        async Task<(string ReaderMnemonic, QrCodeInfo? QrCodeInfo)> DetectAsync(
            string readerMnemonic,
            IQrReaderController controller,
            CancellationToken cancellationToken)
        {
            try
            {
                var qrCodeInfo = await controller.StartDetecting(cancellationToken);
                return (readerMnemonic, qrCodeInfo);
            }
            catch (OperationCanceledException)
            {
                return (readerMnemonic, null);
            }
        }
    }
}