using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using OneOf;
using System.Reactive.Linq;

namespace Domain.InService
{
    public record Authorization(string qrId, int nAuthorizations);
    public class InServiceMgr
    {
        private readonly IValidationMgr validationMgr;        
        private readonly IPassageManager passage;
        private readonly IMMI mmi;
        private readonly QrReaderMgr qrReader;
        private Queue<Authorization> authorizations = new();

        State state = State.Unknown;

        enum State
        {
            Unknown, // only when InServiceMgr is created
            Idle,
            IntrusionWhenIdle,
            IntrusionDuringAuthorizedPassage,
            PassengerInTransit_NoMorePendingAuthorizations,
            SomeAuthorization_s_Queued_ThatHaventBeginTransit
        }

        enum CompoundState
        {
            Unknown,
            Idle,
            IntrusionWhenIdle,
            IntrusionDuringAuthorizedPassage,
            PassengerInTransit_NoMorePendingAuthorizations,
            SomeAuthorization_s_Queued_ThatHaventBeginTransit
        }

        public InServiceMgr(
            IValidationMgr validationMgr,
            IPassageManager passage,            
            IMMI mmi,
            QrReaderMgr qrReader // I obey YAGNI and prefer it over IMediaRdr
            )
        {
            this.validationMgr = validationMgr;
            this.passage = passage;
            this.mmi = mmi;
            this.qrReader = qrReader;

            qrReader.QrCodeStream
                .Subscribe(qr => { QrAppeared(qr); });

            passage.PassageStatusObservable
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x => PassageEvt(x));
        }

        private void PassageEvt(Intrusion x)
        {
            switch (state)
            {
                case State.Unknown:
                    {
                        state = State.IntrusionWhenIdle;
                        mmi.IntrusionWhenIdle(x);
                        break;
                    }
                case State.Idle:
                    {
                        state = State.IntrusionWhenIdle;
                        mmi.IntrusionWhenIdle(x);
                        break;
                    }
                case State.IntrusionWhenIdle:
                    {
                        state = State.IntrusionWhenIdle;
                        mmi.IntrusionWhenIdle(x);
                        break;
                    }                    
                case State.IntrusionDuringAuthorizedPassage:
                    {
                        state = State.IntrusionDuringAuthorizedPassage;
                        mmi.IntrusionDuringAuthorizedPassage(x);
                        break;
                    }
                case State.SomeAuthorization_s_Queued_ThatHaventBeginTransit:
                case State.PassengerInTransit_NoMorePendingAuthorizations:
                    {
                        state = State.IntrusionDuringAuthorizedPassage;
                        mmi.IntrusionDuringAuthorizedPassage(x);
                        break;
                    }
            }
        }

        private void PassageEvt(Fraud x)
        { }

        private void PassageEvt(PassageTimeout x)
        { }

        private void PassageEvt(AuthroizedPassengerSteppedBack x)
        { }

        private void PassageEvt(PassageInProgress x)
        {
            switch(state)
            {
                case State.IntrusionWhenIdle:
                case State.Unknown:
                    // unexpected
                    break;
                case State.IntrusionDuringAuthorizedPassage:
                    //state = State.
                    //mmi.IntrusionCleared();
                    break;
                case State.PassengerInTransit_NoMorePendingAuthorizations:
                    break;
            }
        }

        private void PassageEvt(PassageDone x)
        { }

        private void PassageEvt(OneOf<Intrusion, Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone> x)
        {
            if (x.IsT0)
                PassageEvt(x.AsT0);
            else if (x.IsT1)
                PassageEvt(x.AsT1);
            else if (x.IsT2)
                PassageEvt(x.AsT2);
            else if (x.IsT3)
                PassageEvt(x.AsT3);
            else if (x.IsT4)
                PassageEvt(x.AsT4);
            else if (x.IsT5)
                PassageEvt(x.AsT5);
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