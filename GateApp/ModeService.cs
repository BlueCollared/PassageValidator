using Domain;
using Domain.Services.Modes;

namespace GateApp
{
    public class ModeService : IModeService
    {
        private readonly ModeManager modeMgr;
        private readonly IContextRepository contextRepo;
        public IObservable<Mode> EquipmentModeObservable => modeMgr.EquipmentModeObservable;

        public ModeService(ModeManager modeMgr, IContextRepository contextRepo)
        {
            this.modeMgr = modeMgr;
            this.contextRepo = contextRepo;
        }

        public Mode CurMode => modeMgr.CurMode;

        public void SwitchToMaintenance()
        {
            if (modeMgr.CurMode == Mode.Maintenance)
                return;

            modeMgr.SwitchToMaintenance();
        }

        public void SwitchOutMaintenance()
        {
            if (modeMgr.CurMode != Mode.Maintenance)
                return;

            modeMgr.SwitchOutMaintenance();
        }

        public bool ChangeMode(OpMode mode, TimeSpan timeout)
        {
            if (modeMgr.ModeDemanded == mode)
                return true;

            modeMgr.ModeDemanded = mode;
            contextRepo.SaveMode(mode);
            var sleepDuration = TimeSpan.FromMilliseconds(20);
            TimeSpan totalTimeTaken = TimeSpan.FromMilliseconds(0);
            while (true)
            {
                if (modeMgr.ModeDemanded == mode)
                    return true;
                Thread.Sleep(sleepDuration);
                totalTimeTaken += sleepDuration;
                if (totalTimeTaken > timeout)
                    break;
            }
            return false;
        }
    }
}