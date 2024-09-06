using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.InService;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Domain.Services.Modes
{
    public class ModeManager
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
        private readonly IDisposable _timerSubscription;
        private bool bMaintenacneRequested = false;
        private OpMode opModeDemanded;
        public ISubModeMgr curModeMgr { get; private set; }

        private readonly BehaviorSubject<Mode> EquipmentModeSubject = new BehaviorSubject<Mode>(Mode.AppBooting);
        public IObservable<Mode> EquipmentModeObservable => EquipmentModeSubject.DistinctUntilChanged().AsObservable();
        public Mode CurMode => EquipmentModeSubject.Value;

        // Private constructor that includes the scheduler parameter
        public ModeManager(IQrReaderMgr qrReaderMgr,
                               ValidationMgr validationMgr,
                               GateMgr gateMgr,
                               IScheduler scheduler,
                               int timeToCompleteAppBoot_InSeconds = DEFAULT_TimeToCompleteBoot_InSeconds)
        {
            this.qrReaderMgr = qrReaderMgr;
            this.validationMgr = validationMgr;
            this.gateMgr = gateMgr;

            _timerSubscription = Observable.Timer(TimeSpan.FromSeconds(timeToCompleteAppBoot_InSeconds), scheduler)
                                           .Subscribe(_ => DoModeRelatedX());

            qrReaderMgr.StatusStream.Subscribe(onNext: QrRdrStatusChanged);
            validationMgr.StatusStream.Subscribe(onNext: ValidationSystemStatusChanged);
            gateMgr.StatusStream.Subscribe(onNext: GateStatusChanged);
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
                if (value == OpMode.OOS)
                {
                    if (curModeMgr == null)
                        return;
                    if (curModeMgr is InServiceMgr x)
                    {
                        x.HaltFurtherValidations().Wait(); // TODO: don't wait infinitely.
                        curModeMgr.Dispose();
                        curModeMgr = null;
                    }
                }
            }
        }

        private void QrRdrStatusChanged(QrReaderStatus status)
        {
            equipmentStatus = equipmentStatus with { QrEntry = equipmentStatus.QrEntry.UpdateStatus(status) };
            DoModeRelated();
        }

        private void ValidationSystemStatusChanged(ValidationSystemStatus status)
        {
            equipmentStatus = equipmentStatus with { ValidationAPI = equipmentStatus.ValidationAPI.UpdateStatus(status) };
            DoModeRelated();
        }

        private void GateStatusChanged(GateHwStatus status)
        {
            equipmentStatus = equipmentStatus with { gateStatus = equipmentStatus.gateStatus.UpdateStatus(status) };
            DoModeRelated();
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
            Mode modeAfter = bMaintenacneRequested ? Mode.Maintenance : CalculateMode(equipmentStatus);

            if (modeAfter != modeBefore)
            {
                if (curModeMgr != null)
                    curModeMgr.Dispose();
                SwitchTo(modeAfter);
            }
            EquipmentModeSubject.OnNext(modeAfter);
        }

        private static Mode CalculateMode(EquipmentStatus e)
        {
            bool bQrAvailable = e.QrEntry?.Status?.IsAvailable ?? false;
            bool bValidationAPIAvailable = e.ValidationAPI?.Status?.IsAvailable ?? false;
            bool bGateAvailable = e.gateStatus?.Status?.IsAvailable ?? false;

            return (bQrAvailable && bValidationAPIAvailable && bGateAvailable) ? Mode.InService : Mode.OOO;
        }

        private void SwitchTo(Mode modeAfter)
        {
            switch (modeAfter)
            {
                case Mode.InService:
                    curModeMgr = new InServiceMgr(validationMgr, new PassageMgr(), qrReaderMgr);
                    break;
                    //case Mode.OOO:
                    //    curModeMgr = new OOOMgr(mmi);
                    //    break;
            }
        }

        private bool AreAllStatusesReceived()
        {
            var e = equipmentStatus;
            return e.QrEntry.IsKnown
                && (e.ValidationAPI != null && e.ValidationAPI.Status?.offlineStatus != null)
                && (e.ValidationAPI != null && e.ValidationAPI.Status?.onlineStatus != null)
                && e.gateStatus.IsKnown;
        }

        public void SwitchToMaintenance()
        {
            bMaintenacneRequested = true;
            DoModeRelatedX();
        }

        public void SwitchOutMaintenance()
        {
            bMaintenacneRequested = false;
            DoModeRelatedX();
        }
    }
}
