using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.InService;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using LanguageExt;
using LanguageExt.ClassInstances;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Xml.Schema;

namespace Domain.Services.Modes
{
    public interface IModeQueryService
    {
        IObservable<(Mode, ISubModeMgr)> EquipmentModeObservable { get; }
    }
    // Ideally, when ModeManager is accepting a stream and giving another stream as an output, we shouldn't leave the stream domain
    // But there is so much overhead that  
    public class ModeManager : IModeQueryService
    {
        public const int DEFAULT_TimeToCompleteBoot_InSeconds = 10;

        private readonly IQrReaderMgr qrReaderMgr;
        private readonly ValidationMgr validationMgr;
        private readonly GateMgr gateMgr;
        private readonly ISubModeMgrFactory modeMgrFactory;
        IObservable<Mode> modeCalculatedStream, modeEffectuatedStream; // both may be different. e.g. in maintenace mode, we eject the qr reader, then OOO would be put in modeCalculatedStream, but modeEffectuatedStream would not be disturbed
        //private readonly IDisposable timerAppBootTimeoutSubscr;
        // ---- Begin ------
        Subject<bool> maintAskedSubject = new();
        IConnectableObservable<bool> maintenanceAsked;

        Subject<OpMode> modeAskedSubject = new();
        IConnectableObservable<OpMode> modeRequested;
        //IDisposable maintSubscription; // TODO: make it a local variable
        // ---- End ------

        IObservable<EquipmentStatus> equipmentStatusStream;
        
        //private bool bMaintenanceRequested = false;
        private OpMode opModeDemanded;
        public ISubModeMgr curModeMgr { get; private set; } = new DoNothingModeMgr(Mode.AppBooting);

        private readonly BehaviorSubject<(Mode, ISubModeMgr)> EquipmentModeSubject;
        public IObservable<(Mode, ISubModeMgr)> EquipmentModeObservable => EquipmentModeSubject
            .DistinctUntilChanged()
            .AsObservable();
        public Mode CurMode => EquipmentModeSubject.Value.Item1;

        //defValue will be emitted if no element is pushed within maxTimeToWaitBeforeUsingDefValue of start
        static IObservable<T> ProcessedIObservable<T>(IObservable<T> stream, T defValue, TimeSpan maxTimeToWaitBeforeFallbacking) //where T:ModuleStatus
        {
            return stream
                .Timeout(maxTimeToWaitBeforeFallbacking)
                .Catch(Observable.Return(defValue))  // If no event in 20s, inject NotConnected
                                                     //.Catch(Observable.Return(Option<T>.None))
                .Merge(stream)
                .DistinctUntilChanged();
        }        

        // Private constructor that includes the scheduler parameter
        public ModeManager(IQrReaderMgr qrReaderMgr,
                               ValidationMgr validationMgr,
                               GateMgr gateMgr,
                               ISubModeMgrFactory modeMgrFactory,
                               IScheduler scheduler,
                               int timeToCompleteAppBoot_InSeconds = DEFAULT_TimeToCompleteBoot_InSeconds)
        {
            maintenanceAsked = maintAskedSubject.AsObservable().Replay(1);
            maintenanceAsked.Connect();
            maintAskedSubject.OnNext(false);

            modeRequested = modeAskedSubject.AsObservable().Replay(1);
            modeRequested.Connect();

            curModeMgr = modeMgrFactory.Create(Mode.AppBooting);
            EquipmentModeSubject = new BehaviorSubject<(Mode, ISubModeMgr)>((Mode.AppBooting, curModeMgr));
            this.qrReaderMgr = qrReaderMgr;
            this.validationMgr = validationMgr;
            this.gateMgr = gateMgr;
            this.modeMgrFactory = modeMgrFactory;

            var maxTimeToWaitBeforeFallbacking = TimeSpan.FromSeconds(timeToCompleteAppBoot_InSeconds);
            var qr = qrReaderMgr.StatusStream;
            qr =  ProcessedIObservable(qr, QrReaderStatus.Disconnected, maxTimeToWaitBeforeFallbacking);

            var valid = validationMgr.StatusStream;
            valid = ProcessedIObservable(valid, ValidationSystemStatus.Default, maxTimeToWaitBeforeFallbacking);

            var gate = gateMgr.StatusStream;
            gate = ProcessedIObservable(gate, GateHwStatus.DisConnected, maxTimeToWaitBeforeFallbacking);

            equipmentStatusStream =            
            Observable.CombineLatest(qr, valid, gate, maintenanceAsked, modeRequested, (qr, valid, gate, maint, mod) => new EquipmentStatus(
                qr, valid, gate,
                bMaintAsked: maint,
                modeAsked:mod
                )).ObserveOn(scheduler);            
            
            modeCalculatedStream = equipmentStatusStream
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
                );
        }        

        IDisposable eqptStatus;

        //// Public factory method to create an instance with the default scheduler
        //public static ModeManager Create(IQrReaderMgr qrReaderMgr,
        //                                 ValidationMgr validationMgr,
        //                                 GateMgr gateMgr,
        //                                 int timeToCompleteAppBoot_InSeconds = DEFAULT_TimeToCompleteBoot_InSeconds)
        //{
        //    return new ModeManager(qrReaderMgr, validationMgr, gateMgr, Scheduler.Default, timeToCompleteAppBoot_InSeconds);
        //}

        //// Public factory method for testing purposes
        //public static ModeManager CreateForTesting(IQrReaderMgr qrReaderMgr,
        //                                           ValidationMgr validationMgr,
        //                                           GateMgr gateMgr,
        //                                           IScheduler scheduler,
        //                                           int timeToCompleteAppBoot_InSeconds = DEFAULT_TimeToCompleteBoot_InSeconds)
        //{
        //    return new ModeManager(qrReaderMgr, validationMgr, gateMgr, scheduler, timeToCompleteAppBoot_InSeconds);
        //}

        public OpMode ModeDemanded
        {
            get { return opModeDemanded; }
            set
            {
                modeAskedSubject.OnNext(value);
                //this.opModeDemanded = value;                
            }
        }

        //private void DoModeRelatedX()
        //{
            //Mode modeBefore = CurMode;
            //Mode modeAfter = bMaintenanceRequested ? Mode.Maintenance : CalculateMode(equipmentStatus);

            //if (modeAfter != modeBefore)
            //{
            //    if (curModeMgr is InServiceMgr x)                
            //        x.HaltFurtherValidations().Wait(); // TODO: don't wait infinitely.
                
            //    curModeMgr?.Dispose();
            //    curModeMgr = modeMgrFactory.Create(modeAfter);                
            //}
            //EquipmentModeSubject.OnNext((modeAfter, curModeMgr));
        //}       

        public async Task SwitchToMaintenance()
        {
            await curModeMgr.Stop();
            curModeMgr.Dispose();

            maintAskedSubject.OnNext(true);            
        }

        public Task SwitchOutMaintenance()
        {
            maintAskedSubject.OnNext(false);            
            return Task.CompletedTask;
        }
    }
}
