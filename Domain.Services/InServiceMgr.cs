using Equipment.Core.Message;
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

public record SideState
{
    public static SideState_Idle_Polling Idle_Polling()
    {
        return new SideState_Idle_Polling();
    }

    public static SideState_Idle_Free Idle_Free()
    {
        return new SideState_Idle_Free();
    }

    public static SideState_RejectPassage RejectPassage()
    {
        return new SideState_RejectPassage();
    }

    public static SideState_ValidationInProgress ValidationInProgress(QrCodeInfo info)
    {
        return new SideState_ValidationInProgress(info);
    }

    public static SideState_PassageAuthorized PassageAuthroized(QrCodeInfo info)
    {
        return new SideState_PassageAuthorized(info);
    }

    public static SideState_Prohibited_BecausePassageInProgressFromOtherSide ProhibitedBecausePassageInProgressFromOtherSide()
    {
        return new SideState_Prohibited_BecausePassageInProgressFromOtherSide();
    }

    public static SideState_Prohibited Prohibited()
    {
        return new SideState_Prohibited();
    }

    public static SideState_Intrusion Intrusion()
    {
        return new SideState_Intrusion();
    }

    public static SideState_Fraud Fraud()
    {
        return new SideState_Fraud();
    }
};

public record SideState_Unknown : SideState;
public record SideState_Idle_Polling : SideState;
public record SideState_Idle_Free : SideState;
public record SideState_RejectPassage : SideState;
public record SideState_Fraud : SideState;
public record SideState_Intrusion : SideState;
public record SideState_ValidationInProgress(QrCodeInfo QrCodeInfo) : SideState;
public record SideState_PassageAuthorized(QrCodeInfo QrCodeInfo) : SideState;
public record SideState_Prohibited_BecausePassageInProgressFromOtherSide : SideState;
public record SideState_Prohibited : SideState; // Forbidden/Interdit

public class InServiceMgr : ISubModeMgr, IInServiceMgr
{
    private readonly IValidate validationMgr;    
    private readonly IQrReaderMgr qrMgr;
    private readonly DeviceStatusSubscriber<ActiveFunctionalities> activeFunctions;
    private Queue<Authorization> authorizations = new();

    PassageState state = PassageState.Unknown;    

    private bool IsDisposed;

    public record State
    {
        public static State Idle() { return new Idle(); }
        public static State ValidationAtEntryInProgress(QrCodeInfo info) { return new ValidationAtEntryInProgress(info); }
        public static State ValidationAtExitInProgress(QrCodeInfo info) { return new ValidationAtExitInProgress(info); }
        public static State PassageAuthroizedAtEntry(QrCodeInfo info) { return new PassageAuthroizedAtEntry(info); }
        public static State PassageAuthroizedAtExit(QrCodeInfo info) { return new PassageAuthroizedAtExit(info); }
    };

    public record Idle: State;
    public record ValidationAtEntryInProgress (QrCodeInfo info): State;
    public record ValidationAtExitInProgress(QrCodeInfo info) : State;
    public record PassageAuthroizedAtEntry(QrCodeInfo info) : State;
    public record PassageAuthroizedAtExit(QrCodeInfo info) : State;

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

    BehaviorSubject<State> stateSub = new BehaviorSubject<State>(State.Idle());
    public IObservable<State> StateObservable => stateSub.AsObservable();//Observable.Empty<State>();
    Task tsk;
    
    public InServiceMgr(
        IValidate validationMgr,
        IGateInServiceController passage,            
        IQrReaderMgr qrMgr,
        DeviceStatusSubscriber<ActiveFunctionalities> activeFunctions
        )
    {
        this.validationMgr = validationMgr;
        //this.passage = passage;
        this.qrMgr = qrMgr;
        this.activeFunctions = activeFunctions;

        //passageStatusSubscription = passage.PassageStatusObservable
        //    .ObserveOn(SynchronizationContext.Current)
        //    .Subscribe(x => PassageEvt(x));
        tsk = Task.Run(async() => {
            while (true)
            {
                (string ReaderMnemonic, QrCodeInfo QrCodeInfo) detectionResult;
                try
                {
                    stateSub.OnNext(State.Idle());

                    string pollMode = IQrReaderMgr.Both;
                    if (activeFunctions.curStatus.entry && !activeFunctions.curStatus.exit)
                        pollMode = IQrReaderMgr.Entry;
                    else if (!activeFunctions.curStatus.entry && activeFunctions.curStatus.exit)
                        pollMode = IQrReaderMgr.Exit;

                    detectionResult = await qrMgr.StartDetecting(pollMode, cts.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                if (detectionResult.ReaderMnemonic == IQrReaderMgr.Entry)
                    stateSub.OnNext(State.ValidationAtEntryInProgress(detectionResult.QrCodeInfo));
                else if (detectionResult.ReaderMnemonic == IQrReaderMgr.Exit)
                    stateSub.OnNext(State.ValidationAtExitInProgress(detectionResult.QrCodeInfo));

                QrCodeValidationResult validationResult = validationMgr.Validate(detectionResult.QrCodeInfo);
                if (validationResult != null)
                {
                    if (!validationResult.bGood)
                    { }
                    else
                    {
                        if (detectionResult.ReaderMnemonic == IQrReaderMgr.Entry)
                        {
                            stateSub.OnNext(State.PassageAuthroizedAtEntry(detectionResult.QrCodeInfo));
                            // TODO: ask passageController to Authorise
                            await Task.Delay(5000); // to simulate the delay in passage
                        }
                        else if (detectionResult.ReaderMnemonic == IQrReaderMgr.Exit)
                        {
                            stateSub.OnNext(State.PassageAuthroizedAtExit(detectionResult.QrCodeInfo));
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