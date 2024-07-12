using Domain.InService;
using Domain.Peripherals.Qr;
using Domain.Services.InService;
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
        public IObservable<Mode> EquipmentModeObservable => EquipmentModeSubject.DistinctUntilChanged().AsObservable();
        public Mode CurMode => EquipmentModeSubject.Value;

        public ModeManager(QrReaderMgr qrReaderMgr)
        {
            //eqptStatusSubject = new BehaviorSubject<EquipmentStatus>(new EquipmentStatus());
            equipmentStatus = new();
            this.qrReaderMgr = qrReaderMgr;
            qrReaderMgr.StatusStream.Subscribe(onNext:
                x => { QrRdrStatusChanged(x); }
                );
        }

        private void QrRdrStatusChanged(QrReaderStatus status)
        {
            equipmentStatus = equipmentStatus with { QrEntry = status.bConnected ? ModuleStatus.Good : ModuleStatus.Bad };
            DoModeRelated();
        }

        private void DoModeRelated()
        {
            Mode modeBefore = CurMode;
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
        IValidationMgr validationMgr;
        IPassageManager passageMgr;
        IMMI mmi;
    }
}