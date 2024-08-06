using Domain.InService;
using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using OneOf;
using System.Reactive.Linq;

namespace Domain.Services.InService
{
    public record Authorization(int nAuthorizations);
    public class InServiceMgr : ISubModeMgr, IInServiceMgr
    {
        private readonly ValidationMgr validationMgr;
        private readonly IPassageManager passage;        
        private readonly IQrReaderMgr qrReader;
        private Queue<Authorization> authorizations = new();

        State state = State.Unknown;
        private bool disposedValue;

        IDisposable qrCodeSubscription;
        IDisposable passageStatusSubscription;

        public enum State
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

        public IObservable<State> StateObservable => Observable.Empty<State>();

        public InServiceMgr(
            ValidationMgr validationMgr,
            IPassageManager passage,            
            IQrReaderMgr qrReader // I obey YAGNI and prefer it over IMediaRdr
            )
        {
            this.validationMgr = validationMgr;
            this.passage = passage;            
            this.qrReader = qrReader;

            qrCodeSubscription = qrReader.QrCodeStream
                .Subscribe(qr => { QrAppeared(qr); });

            //passageStatusSubscription = passage.PassageStatusObservable
            //    .ObserveOn(SynchronizationContext.Current)
            //    .Subscribe(x => PassageEvt(x));

            qrReader.StartDetecting();
        }

        public Task HaltFurtherValidations()
        {
            // TODO: correct me
            return Task.CompletedTask;
        }

        private void PassageEvt(Intrusion x)
        {
            switch (state)
            {
                case State.Unknown:
                    {
                        state = State.IntrusionWhenIdle;                        
                        break;
                    }
                case State.Idle:
                    {
                        state = State.IntrusionWhenIdle;                        
                        break;
                    }
                case State.IntrusionWhenIdle:
                    {
                        state = State.IntrusionWhenIdle;                        
                        break;
                    }
                case State.IntrusionDuringAuthorizedPassage:
                    {
                        state = State.IntrusionDuringAuthorizedPassage;                        
                        break;
                    }
                case State.SomeAuthorization_s_Queued_ThatHaventBeginTransit:
                case State.PassengerInTransit_NoMorePendingAuthorizations:
                    {
                        state = State.IntrusionDuringAuthorizedPassage;                        
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
            switch (state)
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

        public override void Dispose()
        {
            IsDisposed = true;
        }
    }

    //public interface IInServiceMgrFactory
    //{
    //    IInServiceMgr Create();
    //}
}