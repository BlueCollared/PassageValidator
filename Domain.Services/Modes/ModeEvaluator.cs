using Equipment.Core.Message;
using EtGate.Domain;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;
using LanguageExt;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Domain.Services.Modes;
public class ModeEvaluator : IModeManager
{
    public const int DEFAULT_TimeToCompleteBoot_InSeconds = 20;
    private readonly IDeviceStatusPublisher<(Mode, bool)> modePub;
    private readonly IDeviceStatusPublisher<ActiveFunctionalities> activeFuncs;

    //IObservable<Mode> modeEffectuatedStream; // both may be different. e.g. in maintenace mode, we eject the qr reader, then OOO would be put in modeCalculatedStream, but modeEffectuatedStream would not be disturbed        

    Subject<bool> maintAskedSubject = new();
    IConnectableObservable<bool> maintenanceAsked;

    Subject<OpMode> modeAskedSubject = new();
    IConnectableObservable<OpMode> modeRequested;

    IObservable<EquipmentStatus> equipmentStatusStream;

    private OpMode opModeDemanded;

    Subject<Unit> forceTimeoutSubject = new Subject<Unit>();

    //defValue will be emitted if no element is pushed within maxTimeToWaitBeforeUsingDefValue of start
    IObservable<T> ProcessedIObservable<T>(IObservable<T> stream, T defValue, TimeSpan maxTimeToWaitBeforeFallbacking, IScheduler scheduler) //where T:ModuleStatus
    {
        var timeoutSignal = forceTimeoutSubject.Select(_ => defValue);

        return stream
            .Merge(timeoutSignal)
            .Timeout(maxTimeToWaitBeforeFallbacking, scheduler)
            .Catch(Observable.Return(defValue))  // If no event in `maxTimeToWaitBeforeFallbacking` seconds, inject NotConnected
                                                 //.Catch(Observable.Return(Option<T>.None))
            .Merge(stream)
            .DistinctUntilChanged();
    }

    public ModeEvaluator(PeripheralStatuses ps,
                           IDeviceStatusPublisher<(Mode, bool)> modePub,
                           IDeviceStatusPublisher<ActiveFunctionalities> activeFuncsPub,
                           IScheduler scheduler,
                           int timeToCompleteAppBoot_InSeconds = DEFAULT_TimeToCompleteBoot_InSeconds)
    {
        var qr = ps.qr;
        var offline = ps.offline;
        var online = ps.online;
        var gate = ps.gate;

        maintenanceAsked = maintAskedSubject.AsObservable().Replay(1);
        maintenanceAsked.Connect();
        maintAskedSubject.OnNext(false);

        modeRequested = modeAskedSubject.AsObservable().Replay(1);
        modeRequested.Connect();
        modeAskedSubject.OnNext(OpMode.InService); // TODO: correct it. It should be injected into the constructor,  the client should take it from a context file       

        this.modePub = modePub;

        modePub?.Publish((Mode.AppBooting, true));

        this.activeFuncs = activeFuncsPub;

        var maxTimeToWaitBeforeFallbacking = TimeSpan.FromSeconds(timeToCompleteAppBoot_InSeconds);

        var qr_ = ProcessedIObservable(qr.Messages, QrReaderStatus.Disconnected, maxTimeToWaitBeforeFallbacking, scheduler);

        var offline_ = ProcessedIObservable(offline.Messages, OfflineValidationSystemStatus.Obsolete, maxTimeToWaitBeforeFallbacking, scheduler);

        var online_ = ProcessedIObservable(online.Messages, OnlineValidationSystemStatus.Disconnected, maxTimeToWaitBeforeFallbacking, scheduler);

        var gate_ = ProcessedIObservable(gate.Messages, GateHwStatus.DisConnected, maxTimeToWaitBeforeFallbacking, scheduler);

        equipmentStatusStream =
        Observable.CombineLatest(qr_, offline_, online_, gate_, maintenanceAsked, modeRequested, (qr, offline, online, gate, maint, mod) => new EquipmentStatus(
            qr, offline, online, gate,
            bMaintAsked: maint,
            modeAsked: mod
            ));

        var fnModeCal = (EquipmentStatus e) =>
            {
                if (e.bMaintAsked)
                    return Mode.Maintenance;

                bool bCanBeInService = e.QrEntry.IsAvailable && e.gateStatus.IsAvailable && (e.offline.IsAvailable || e.online.IsAvailable);
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
            };
        equipmentStatusStream.Subscribe(x =>
        {
            var newMode = fnModeCal(x);
            modePub?.Publish((newMode, newMode == Mode.Maintenance || newMode == Mode.Emergency));
        });

        //qr_.Subscribe(x => Debug.WriteLine($"{DateTime.Now} qr {x}"));
        //gate_.Subscribe(x => Debug.WriteLine($"{DateTime.Now} gate {x}"));
        //offline_.Subscribe(x => Debug.WriteLine($"{DateTime.Now} validation {x}"));
        //offline.Messages.Subscribe(x => { });

        equipmentStatusStream.Subscribe(x => Debug.WriteLine($"{DateTime.Now} equipmentStatusStream {equipmentStatusStream}"));

        //modeEffectuatedStream.Subscribe(x => Debug.WriteLine($"{DateTime.Now} modeEffectuatedStream {x}"));
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
        maintAskedSubject.OnNext(true);
        await Task.CompletedTask; // TODO: correct it
    }

    public Task SwitchOutMaintenance()
    {
        maintAskedSubject.OnNext(false);
        return Task.CompletedTask; // TODO: correct it
    }
}