using Domain;
using Domain.Services.InService;
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

        public ISubModeMgr curModeMgr => modeMgr.curModeMgr;

        public async Task SwitchToMaintenance()
        {
            if (modeMgr.CurMode == Mode.Maintenance)
                return;

            if (curModeMgr != null)
            {
                await curModeMgr.Stop();
                curModeMgr.Dispose();
            }
            modeMgr.SwitchToMaintenance();
        }

        public async Task SwitchOutMaintenance()
        {
            if (modeMgr.CurMode != Mode.Maintenance)
                return;

            await curModeMgr.Stop();
            curModeMgr.Dispose();

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