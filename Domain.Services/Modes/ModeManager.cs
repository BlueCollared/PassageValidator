using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.InService;
using EtGate.Domain.ValidationSystem;
using LanguageExt;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Domain.Services.Modes;

public interface IModeQueryService
{
    IObservable<(Mode, ISubModeMgr)> EqptModeObservable { get; }
}

public class ModeManager : IModeQueryService
{
    public const int DEFAULT_TimeToCompleteBoot_InSeconds = 10;
    private readonly ISubModeMgrFactory modeMgrFactory;
    IObservable<Mode> modeEffectuatedStream; // both may be different. e.g. in maintenace mode, we eject the qr reader, then OOO would be put in modeCalculatedStream, but modeEffectuatedStream would not be disturbed        

    Subject<bool> maintAskedSubject = new();
    IConnectableObservable<bool> maintenanceAsked;

    Subject<OpMode> modeAskedSubject = new();
    IConnectableObservable<OpMode> modeRequested;       

    IObservable<EquipmentStatus> equipmentStatusStream;        
    
    private OpMode opModeDemanded;
    public ISubModeMgr curModeMgr => EqptModeSubject.Value.Item2;    

    private readonly BehaviorSubject<(Mode, ISubModeMgr)> EqptModeSubject;
    public IObservable<(Mode, ISubModeMgr)> EqptModeObservable => EqptModeSubject
        .DistinctUntilChanged()
        .AsObservable();
    public Mode CurMode => EqptModeSubject.Value.Item1;
    Subject<Unit> forceTimeoutSubject = new Subject<Unit>();

    //defValue will be emitted if no element is pushed within maxTimeToWaitBeforeUsingDefValue of start
    IObservable<T> ProcessedIObservable<T>(IObservable<T> stream, T defValue, TimeSpan maxTimeToWaitBeforeFallbacking) //where T:ModuleStatus
    {
        var timeoutSignal = forceTimeoutSubject.Select(_ => defValue);

        return stream
            .Merge(timeoutSignal)
            .Timeout(maxTimeToWaitBeforeFallbacking)
            .Catch(Observable.Return(defValue))  // If no event in 20s, inject NotConnected
                                                 //.Catch(Observable.Return(Option<T>.None))
            .Merge(stream)
            .DistinctUntilChanged();
    }
    
    public ModeManager(IObservable<QrReaderStatus> qrReaderMgr, // TODO: replace with status stream
                           IObservable<ValidationSystemStatus> validationMgr, // TODO: replace with status stream
                           IObservable<GateHwStatus> gateMgr, // TODO: replace with status stream
                           ISubModeMgrFactory modeMgrFactory,
                           IScheduler scheduler,
                           int timeToCompleteAppBoot_InSeconds = DEFAULT_TimeToCompleteBoot_InSeconds)
    {
        maintenanceAsked = maintAskedSubject.AsObservable().Replay(1);
        maintenanceAsked.Connect();
        maintAskedSubject.OnNext(false);

        modeRequested = modeAskedSubject.AsObservable().Replay(1);
        modeRequested.Connect();
        modeAskedSubject.OnNext(OpMode.InService); // TODO: correct it. It should be injected into the constructor,  the client should take it from a context file
        
        EqptModeSubject = new BehaviorSubject<(Mode, ISubModeMgr)>((Mode.AppBooting, modeMgrFactory.Create(Mode.AppBooting)));
        //this.qrReaderMgr = qrReaderMgr;
        //this.validationMgr = validationMgr;
        //this.gateMgr = gateMgr;
        this.modeMgrFactory = modeMgrFactory;

        var maxTimeToWaitBeforeFallbacking = TimeSpan.FromSeconds(timeToCompleteAppBoot_InSeconds);
        var qr = qrReaderMgr;
        qr =  ProcessedIObservable(qr, QrReaderStatus.Disconnected, maxTimeToWaitBeforeFallbacking);            

        var valid = validationMgr;
        valid = ProcessedIObservable(valid, ValidationSystemStatus.Default, maxTimeToWaitBeforeFallbacking);

        var gate = gateMgr;
        gate = ProcessedIObservable(gate, GateHwStatus.DisConnected, maxTimeToWaitBeforeFallbacking);

        equipmentStatusStream =
        Observable.CombineLatest(qr, valid, gate, maintenanceAsked, modeRequested, (qr, valid, gate, maint, mod) => new EquipmentStatus(
            qr, valid, gate,
            bMaintAsked: maint,
            modeAsked:mod
            ))
        //.ObserveOn(scheduler) // TODO: It was causing problems in running tests; they got blocked. Investigate. But w/o this, of course the test AppBootingPhase_Timeout fails
        ;

        modeEffectuatedStream = equipmentStatusStream
            .Select((EquipmentStatus e) =>
            {
                if (e.bMaintAsked)
                    return Mode.Maintenance;

                bool bCanBeInService = e.QrEntry.IsAvailable && e.gateStatus.IsAvailable && e.ValidationAPI.IsAvailable;
                switch (e.modeAsked)
                {
                    case OpMode.Emergency:
                        return Mode.Emergency;
                    case OpMode.InService:
                        return bCanBeInService ? Mode.InService : Mode.OOO;
                    case OpMode.OOS:
                        return bCanBeInService ? Mode.OOS : Mode.OOO;
                    default:
                        throw new NotImplementedException();
                }
            }
            ).DistinctUntilChanged();

        modeEffectuatedStream.ForEachAsync(async x => {
            await curModeMgr.Stop(bImmediate: CurMode == Mode.Emergency);
            curModeMgr.Dispose();            
            
            EqptModeSubject.OnNext((x, modeMgrFactory.Create(x)));
        });

        qr.Subscribe(x => Debug.WriteLine($"qr {x}"));
        gate.Subscribe(x => Debug.WriteLine($"gate {x}"));
        valid.Subscribe(x => Debug.WriteLine($"validation {x}"));

        equipmentStatusStream.Subscribe(x => Debug.WriteLine($"equipmentStatusStream {equipmentStatusStream}"));

        modeEffectuatedStream.Subscribe(x => Debug.WriteLine($"modeEffectuatedStream {x}"));
    }

    IDisposable eqptStatus;

    public OpMode ModeDemanded
    {
        get { return opModeDemanded; }
        set
        {
            modeAskedSubject.OnNext(value);                
        }
    }

    public async Task SwitchToMaintenance()
    {
        forceTimeoutSubject.OnNext(Unit.Default);

        await curModeMgr.Stop(bImmediate:false);
        curModeMgr.Dispose();

        maintAskedSubject.OnNext(true);            
    }

    public Task SwitchOutMaintenance()
    {
        maintAskedSubject.OnNext(false);
        return Task.CompletedTask;
    }
}