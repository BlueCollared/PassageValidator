using Domain.Peripherals.Qr;
using EtGate.Domain.Passage.PassageEvts;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using OneOf;
using System.Reactive.Linq;

namespace Domain.Services.InService;

public record Authorization(int nAuthorizations);
public class InServiceMgr : ISubModeMgr, IInServiceMgr
{
    private readonly ValidationMgr validationMgr;
    private readonly IGateInServiceController passage;        
    private readonly IQrReaderMgr qrReader;
    private Queue<Authorization> authorizations = new();

    State state = State.Unknown;
    private bool disposedValue;

    IDisposable qrCodeSubscription;
    IDisposable passageStatusSubscription;
    private bool IsDisposed;

    public enum State
    {
        Unknown, // only when InServiceMgr is created
        Idle,
        IntrusionAtExitWhenIdle,
        IntrusionAtEntryWhenIdle,
        IntrusionDuringAuthorizedPassage,
        PassengerInTransit_NoMorePendingAuthorizations,
        SomeAuthorization_s_Queued_ThatHaventBeginTransit
    }

    public enum CompoundState
    {
        Unknown,
        Idle,
        IntrusionOnMySideWhenIdle,
        IntrusionOnOtherSideWhenIdle,
        IntrusionDuringAuthorizedPassage,
        PassengerInTransit_NoMorePendingAuthorizations,
        SomeAuthorization_s_Queued_ThatHaventBeginTransit
    }

    public IObservable<State> StateObservable => Observable.Empty<State>();

    public InServiceMgr(
        ValidationMgr validationMgr,
        IGateInServiceController passage,            
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

    private void PassageEvt(EtGate.Domain.Passage.PassageEvts.Intrusion x)
    {
        switch (state)
        {
            case State.Unknown:
                {
                    state = State.IntrusionAtEntryWhenIdle;                        
                    break;
                }
            case State.Idle:
                {
                    state = State.IntrusionAtEntryWhenIdle;                        
                    break;
                }
            case State.IntrusionAtEntryWhenIdle:
                {
                    state = State.IntrusionAtEntryWhenIdle;                        
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

    private void PassageEvt(EtGate.Domain.Passage.PassageEvts.Fraud x)
    { }

    private void PassageEvt(PassageTimeout x)
    { }

    private void PassageEvt(AuthroizedPassengerSteppedBack x)
    { }

    private void PassageEvt(EtGate.Domain.Passage.PassageEvts.OpenDoor x)
    {
        switch (state)
        {
            case State.IntrusionAtEntryWhenIdle:
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

    private void PassageEvt(OneOf<EtGate.Domain.Passage.PassageEvts.Intrusion, EtGate.Domain.Passage.PassageEvts.Fraud, EtGate.Domain.Passage.PassageEvts.OpenDoor, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone> x)
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

    public void Dispose()
    {
        IsDisposed = true;
    }

    public Task Stop()
    {
        throw new NotImplementedException();
    }
}

//public interface IInServiceMgrFactory
//{
//    IInServiceMgr Create();
//}