
using Domain;

namespace GateApp
{
    public class ModeChangeService
    {
        private readonly Domain.Services.Modes.ModeManager modeMgr;
        private readonly IContextRepository contextRepo;

        public ModeChangeService(Domain.Services.Modes.ModeManager modeMgr, IContextRepository contextRepo)
        {
            this.modeMgr = modeMgr;
            this.contextRepo = contextRepo;
        }

        public void InterruptForEnteringMaintenace()
        { }

        public void InterruptForExitingMaintenace()
        { }

        public bool ChangeMode (OpMode mode, TimeSpan timeout)
        {
            if (modeMgr.ModeDemanded == mode)
                return true;

            modeMgr.ModeDemanded = mode;
            contextRepo.SaveMode(mode);
            var sleepDuration = TimeSpan.FromMilliseconds(20);
            TimeSpan totalTimeTaken = TimeSpan.FromMilliseconds(0);
            while(true)
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