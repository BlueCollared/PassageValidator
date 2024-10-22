using Domain;
using Domain.Services.Modes;
using GateApp;

namespace EtGate.GateApp
{
    internal class ModeServiceRemoteAgent
    {
        private readonly ModeManager modeMgr;
        private readonly IContextRepository contextRepo;

        public ModeServiceRemoteAgent(ModeManager modeMgr, IContextRepository contextRepo)
        {
            this.modeMgr = modeMgr;
            this.contextRepo = contextRepo;
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
