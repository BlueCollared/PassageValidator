using Domain.Services.Modes;
using EtGate.Domain;
using GateApp;

namespace EtGate.GateApp
{
    internal class ModeServiceRemoteAgent
    {
        private readonly ModeEvaluator modeMgr;
        private readonly IContextRepository contextRepo;

        public ModeServiceRemoteAgent(ModeEvaluator modeMgr, IContextRepository contextRepo)
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
