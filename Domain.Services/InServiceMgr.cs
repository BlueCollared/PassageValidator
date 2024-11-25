using EtGate.Domain;
using EtGate.Domain.Passage.PassageEvts;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using OneOf;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Domain.Services.InService;

public record Authorization(int nAuthorizations);
public class InServiceMgr : ISubModeMgr, IInServiceMgr
{
    private readonly ValidationMgr validationMgr;
    //private readonly IGateInServiceController passage;        
    private readonly IQrReaderMgr qrMgr;
    private Queue<Authorization> authorizations = new();

    PassageState state = PassageState.Unknown;    

    private bool IsDisposed;

    public enum State
    {
        Unknown, // only when InServiceMgr is created
        Idle,
        ValidationAtEntryInProgress,
        ValidationAtExitInProgress,
        PassageAuthroizedAtEntry,
        PassageAuthroizedAtExit,
    }

    public enum PassageState
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
    BehaviorSubject<State> stateSub = new BehaviorSubject<State>(State.Idle);
    public IObservable<State> StateObservable => stateSub.AsObservable();//Observable.Empty<State>();
    Task tsk;
    public InServiceMgr(
        ValidationMgr validationMgr,
        //IGateInServiceController passage,            
        IQrReaderMgr qrMgr
        )
    {
        this.validationMgr = validationMgr;
        //this.passage = passage;            
        this.qrMgr = qrMgr;

        //passageStatusSubscription = passage.PassageStatusObservable
        //    .ObserveOn(SynchronizationContext.Current)
        //    .Subscribe(x => PassageEvt(x));
        tsk = Task.Run(async() => {
            while (true)
            {
                (string ReaderMnemonic, QrCodeInfo QrCodeInfo) detectionResult;
                try
                {
                    stateSub.OnNext(State.Idle);
                    detectionResult = await qrMgr.StartDetecting(IQrReaderMgr.Entry, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                if (detectionResult.ReaderMnemonic == IQrReaderMgr.Entry)
                    stateSub.OnNext(State.ValidationAtEntryInProgress);
                else if (detectionResult.ReaderMnemonic == IQrReaderMgr.Exit)
                    stateSub.OnNext(State.ValidationAtExitInProgress);

                QrCodeValidationResult validationResult = validationMgr.Validate(detectionResult.QrCodeInfo);
                if (validationResult != null)
                {
                    if (!validationResult.bGood)
                    { }
                    else
                    {
                        if (detectionResult.ReaderMnemonic == IQrReaderMgr.Entry)
                        {
                            stateSub.OnNext(State.PassageAuthroizedAtEntry);
                            // TODO: ask passageController to Authorise
                            await Task.Delay(5000); // to simulate the delay in passage
                        }
                        else if (detectionResult.ReaderMnemonic == IQrReaderMgr.Exit)
                        {
                            stateSub.OnNext(State.PassageAuthroizedAtExit);
                            // TODO: ask passageController to Authorise
                            await Task.Delay(5000); // to simulate the delay in passage
                        }
                    }
                }
            }
        });
    }

    CancellationTokenSource cts = new();

    public Task HaltFurtherValidations()
    {
        // TODO: correct me
        return Task.CompletedTask;
    }

    private void PassageEvt(EtGate.Domain.Passage.PassageEvts.Intrusion x)
    {
        switch (state)
        {
            case PassageState.Unknown:
                {
                    state = PassageState.IntrusionAtEntryWhenIdle;                        
                    break;
                }
            case PassageState.Idle:
                {
                    state = PassageState.IntrusionAtEntryWhenIdle;                        
                    break;
                }
            case PassageState.IntrusionAtEntryWhenIdle:
                {
                    state = PassageState.IntrusionAtEntryWhenIdle;                        
                    break;
                }
            case PassageState.IntrusionDuringAuthorizedPassage:
                {
                    state = PassageState.IntrusionDuringAuthorizedPassage;                        
                    break;
                }
            case PassageState.SomeAuthorization_s_Queued_ThatHaventBeginTransit:
            case PassageState.PassengerInTransit_NoMorePendingAuthorizations:
                {
                    state = PassageState.IntrusionDuringAuthorizedPassage;                        
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
            case PassageState.IntrusionAtEntryWhenIdle:
            case PassageState.Unknown:
                // unexpected
                break;
            case PassageState.IntrusionDuringAuthorizedPassage:
                //state = State.
                //mmi.IntrusionCleared();
                break;
            case PassageState.PassengerInTransit_NoMorePendingAuthorizations:
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

    public Task Stop(bool bImmediate)
    {
        throw new NotImplementedException();
    }
}

//public interface IInServiceMgrFactory
//{
//    IInServiceMgr Create();
//}