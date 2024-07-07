using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using System.Reactive.Linq;

namespace Domain.InService
{
    public record Authorization(string qrId, int nAuthorizations);
    public class InServiceMgr
    {
        private readonly IValidationMgr validationMgr;
        private readonly IPassageController passage;
        private readonly QrReaderMgr qrReader;
        private Queue<Authorization> authorizations = new();

        State state = State.Unknown;

        enum State
        {
            Unknown,
            Idle,
            IntrusionWhenIdle,
            PassengerInTransit_NoMorePendingAuthorizations,
            SomeAuthorization_s_Queued_ThatHaventBeginTransit
        }

        public InServiceMgr(
            IValidationMgr validationMgr, 
            IPassageController passage,
            QrReaderMgr qrReader // I obey YAGNI and prefer it over IMediaRdr
            )
        {
            this.validationMgr = validationMgr;
            this.passage = passage;
            this.qrReader = qrReader;

            ((IObservable<QrCodeInfo>)qrReader)
                .Subscribe(qr => { QrAppeared(qr); });
        }

        private void QrAppeared(QrCodeInfo qr)
        {
            throw new NotImplementedException();
        }

        bool Authorize(Authorization auth)
        {
            throw new NotImplementedException();
            if (authorizations.Count > 2)
            { }
        }
    }
}