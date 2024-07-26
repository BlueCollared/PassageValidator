using Domain.InService;
using Domain.Peripherals.Qr;
using Domain.Services.InService;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Domain.Services.Modes
{
    public class ModeManager
    {
        // don't see any value for the mode manager to be the aggregator of the status. Open for review
        //private readonly BehaviorSubject<EquipmentStatus> eqptStatusSubject;
        private EquipmentStatus equipmentStatus = new EquipmentStatus(
        new QrReaderStatus_(),
        new ValidationSystemStatus_()
    );

        private readonly QrReaderMgr qrReaderMgr;
        BehaviorSubject<Mode> EquipmentModeSubject = new BehaviorSubject<Mode>(Mode.AppBooting);
        OpMode opModeDemanded;
        public IObservable<Mode> EquipmentModeObservable => EquipmentModeSubject.DistinctUntilChanged().AsObservable();
        public Mode CurMode => EquipmentModeSubject.Value;       
        
        public ModeManager(QrReaderMgr qrReaderMgr, 
            ValidationMgr validationMgr, 
            IPassageManager passageMgr,
            OpMode opModeDemanded = OpMode.InService)
        {
            //equipmentStatus = new();
            this.qrReaderMgr = qrReaderMgr;
            this.validationMgr = validationMgr;
            this.passageMgr = passageMgr;

            qrReaderMgr.StatusStream.Subscribe(onNext:
                x => { QrRdrStatusChanged(x); }
                );

            validationMgr.StatusStream.Subscribe(onNext: x => {
                ValidationSystemStatusChanged(x);
            });

            //validationMgr.
            
            this.opModeDemanded = opModeDemanded;
        }

        public void InterruptForEnteringMaintenace()
        { }

        public void InterruptForExitingMaintenace()
        { }

        public OpMode ModeDemanded
        {
            get { return opModeDemanded; }
            set {
                // TODO
                this.opModeDemanded = value;
                if (value == OpMode.OOS)
                {                    
                    if (curModeMgr == null)
                        return;
                    if (curModeMgr is InServiceMgr x)
                    {                        
                        // we don't want to corrupt the public API with async. So, we use Wait()
                        x.HaltFurtherValidations().Wait(); // TODO: don't wait infinitly.
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

        private void DoModeRelated()
        {
            if (CurMode == Mode.AppBooting)
                if (!AreAllStatusesReceived())
                    return;

            Mode modeBefore = CurMode;//GetMode();//CurMode;
            Mode modeAfter = CalculateMode();

            EquipmentModeSubject.OnNext(modeAfter);
            if (modeAfter != modeBefore)
            {
                if (curModeMgr != null)
                    curModeMgr.Dispose();
                SwitchTo(modeAfter);
            }

            EquipmentModeSubject.OnNext(modeAfter);
        }

        private Mode CalculateMode()
        {
            Mode modeAfter;
            var e = equipmentStatus;
            if (e.QrEntry.Status.IsAvailable && e.ValidationAPI.Status.IsAvailable)
                modeAfter = Mode.InService;
            else
                modeAfter = Mode.OOO;
            return modeAfter;
        }

        private void SwitchTo(Mode modeAfter)
        {
            switch (modeAfter)
            {
                case Mode.InService:
                    curModeMgr = new InServiceMgr(validationMgr, passageMgr, mmi, qrReaderMgr);
                    break;
                case Mode.OOO:
                    curModeMgr = new OOOMgr(mmi);
                    break;
            }
        }

        private bool AreAllStatusesReceived()
        {
            var e = equipmentStatus;
            bool allStatusesReceived = e.QrEntry.IsKnown
                && (e.ValidationAPI != null && e.ValidationAPI.Status?.offlineStatus != null)// TODO: I didn't want to use null. That's why IsKnown was introduced. But seems can't do without null easily
                && (e.ValidationAPI != null && e.ValidationAPI.Status?.onlineStatus != null);
            return allStatusesReceived;
        }

        ISubModeMgr curModeMgr;
        ValidationMgr validationMgr;
        IPassageManager passageMgr;
        IMMI mmi;
    }
}