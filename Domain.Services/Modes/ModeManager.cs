using Domain.Services.InService;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Domain.Services.Modes
{
    public interface IModeQueryService
    {
        IObservable<(Mode, ISubModeMgr)> EquipmentModeObservable { get; }
    }

    public class ModeManager : IModeQueryService
    {
        public const int DEFAULT_TimeToCompleteBoot_InSeconds = 10;

        private EquipmentStatus equipmentStatus = new EquipmentStatus(
            new QrReaderStatus_(),
            new ValidationSystemStatus_(),
            new GateHwStatus_()
        );

        private readonly IQrReaderMgr qrReaderMgr;
        private readonly ValidationMgr validationMgr;
        private readonly GateMgr gateMgr;
        private readonly ISubModeMgrFactory modeMgrFactory;
        private readonly IDisposable timerAppBootTimeoutSubscr;
        private bool bMaintenanceRequested = false;
        private OpMode opModeDemanded;
        public ISubModeMgr curModeMgr { get; private set; } = new DoNothingModeMgr(Mode.AppBooting);

        private readonly BehaviorSubject<(Mode, ISubModeMgr)> EquipmentModeSubject;
        public IObservable<(Mode, ISubModeMgr)> EquipmentModeObservable => EquipmentModeSubject
            .DistinctUntilChanged()
            .AsObservable();
        public Mode CurMode => EquipmentModeSubject.Value.Item1;

        // Private constructor that includes the scheduler parameter
        public ModeManager(IQrReaderMgr qrReaderMgr,
                               ValidationMgr validationMgr,
                               GateMgr gateMgr,
                               ISubModeMgrFactory modeMgrFactory,
                               IScheduler scheduler,
                               int timeToCompleteAppBoot_InSeconds = DEFAULT_TimeToCompleteBoot_InSeconds)
        {
            curModeMgr = modeMgrFactory.Create(Mode.AppBooting);
            EquipmentModeSubject = new BehaviorSubject<(Mode, ISubModeMgr)>((Mode.AppBooting, curModeMgr));
            this.qrReaderMgr = qrReaderMgr;
            this.validationMgr = validationMgr;
            this.gateMgr = gateMgr;
            this.modeMgrFactory = modeMgrFactory;
            timerAppBootTimeoutSubscr = Observable.Timer(TimeSpan.FromSeconds(timeToCompleteAppBoot_InSeconds), scheduler)
                                           .Subscribe(_ => DoModeRelatedX());

            qrReaderMgr.StatusStream.ObserveOn(scheduler).Subscribe(onNext: x =>
            {
                equipmentStatus = equipmentStatus with { QrEntry = equipmentStatus.QrEntry.UpdateStatus(x) };
                DoModeRelated();
            });

            validationMgr.StatusStream.ObserveOn(scheduler).Subscribe(onNext: x =>
            {
                equipmentStatus = equipmentStatus with { ValidationAPI = equipmentStatus.ValidationAPI.UpdateStatus(x) };
                DoModeRelated();
            });

            gateMgr.StatusStream.ObserveOn(scheduler).Subscribe(onNext: x =>
            {
                equipmentStatus = equipmentStatus with { gateStatus = equipmentStatus.gateStatus.UpdateStatus(x) };
                DoModeRelated();
            });
        }

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
                this.opModeDemanded = value;
                
                if (bMaintenanceRequested)
                    return;
            }
        }

        private void DoModeRelated()
        {
            if (CurMode == Mode.AppBooting)
                if (!AreAllStatusesReceived())
                    return;

            DoModeRelatedX();
        }

        private void DoModeRelatedX()
        {
            Mode modeBefore = CurMode;
            Mode modeAfter = bMaintenanceRequested ? Mode.Maintenance : CalculateMode(equipmentStatus);

            if (modeAfter != modeBefore)
            {
                if (curModeMgr is InServiceMgr x)                
                    x.HaltFurtherValidations().Wait(); // TODO: don't wait infinitely.
                
                curModeMgr?.Dispose();
                curModeMgr = modeMgrFactory.Create(modeAfter);                
            }
            EquipmentModeSubject.OnNext((modeAfter, curModeMgr));
        }

        private static Mode CalculateMode(EquipmentStatus e)
        {
            bool bQrAvailable = e.QrEntry?.Status?.IsAvailable ?? false;
            bool bValidationAPIAvailable = e.ValidationAPI?.Status?.IsAvailable ?? false;
            bool bGateAvailable = e.gateStatus?.Status?.IsAvailable ?? false;

            return (bQrAvailable && bValidationAPIAvailable && bGateAvailable) ? Mode.InService : Mode.OOO;
        }

        private bool AreAllStatusesReceived()
        {
            var e = equipmentStatus;
            return e.QrEntry.IsKnown
                && (e.ValidationAPI != null && e.ValidationAPI.Status?.offlineStatus != null)
                && (e.ValidationAPI != null && e.ValidationAPI.Status?.onlineStatus != null)
                && e.gateStatus.IsKnown;
        }

        public async Task SwitchToMaintenance()
        {
            await curModeMgr.Stop();
            curModeMgr.Dispose();

            bMaintenanceRequested = true;
            DoModeRelatedX();
        }

        public Task SwitchOutMaintenance()
        {
            bMaintenanceRequested = false;
            DoModeRelatedX();
            return Task.CompletedTask;
        }
    }
}
