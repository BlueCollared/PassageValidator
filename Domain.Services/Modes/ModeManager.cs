using Domain.InService;
using Domain.Peripherals.Qr;
using Domain.Services.InService;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Domain.Services.Modes
{
    public class ModeManager
    {
        // don't see any value for the mode manager to be the aggregator of the status. Open for review
        //private readonly BehaviorSubject<EquipmentStatus> eqptStatusSubject;
        EquipmentStatus equipmentStatus;
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
            equipmentStatus = new();
            this.qrReaderMgr = qrReaderMgr;
            this.validationMgr = validationMgr;
            this.passageMgr = passageMgr;

            qrReaderMgr.StatusStream.Subscribe(onNext:
                x => { QrRdrStatusChanged(x); }
                );
            
            this.opModeDemanded = opModeDemanded;
        }

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
            equipmentStatus = equipmentStatus with { QrEntry = status.bConnected ? ModuleStatus.Good : ModuleStatus.Bad };
            DoModeRelated();
        }

        private void DoModeRelated()
        {
            Mode modeBefore = CurMode;//GetMode();//CurMode;
            Mode modeAfter;
            if (equipmentStatus.QrEntry == ModuleStatus.Good)
                modeAfter = Mode.InService;
            else
                modeAfter = Mode.OOO;

            EquipmentModeSubject.OnNext(modeAfter);
            if (modeAfter != modeBefore)
            {
                if (curModeMgr != null)
                    curModeMgr.Dispose();
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

            EquipmentModeSubject.OnNext(modeAfter);
        }

        ISubModeMgr curModeMgr;
        ValidationMgr validationMgr;
        IPassageManager passageMgr;
        IMMI mmi;
    }
}