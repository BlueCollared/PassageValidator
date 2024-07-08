using Domain.Peripherals.Qr;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Domain.Services.Modes
{
    public class ModeManager
    {
        private readonly BehaviorSubject<EquipmentStatus> eqptStatusSubject;
        private readonly QrReaderMgr qrReaderMgr;
        BehaviorSubject<Mode> EquipmentModeSubject = new BehaviorSubject<Mode>(Mode.AppBooting);
        public IObservable<Mode> EquipmentModeObservable => EquipmentModeSubject.DistinctUntilChanged().AsObservable();
        public Mode CurMode => EquipmentModeSubject.Value;        

        public ModeManager(QrReaderMgr qrReaderMgr)
        {
            eqptStatusSubject = new BehaviorSubject<EquipmentStatus>(new EquipmentStatus());
            this.qrReaderMgr = qrReaderMgr;
            qrReaderMgr.StatusStream.Subscribe(onNext:
                x => { QrRdrStatusChanged(x.status); }
                );
        }

        private void QrRdrStatusChanged(QrReaderStatus status)
        {            
            Mode modeNew = Mode.InService; // TODO: determine it correctly
            EquipmentModeSubject.OnNext(modeNew);
        }
    }
}