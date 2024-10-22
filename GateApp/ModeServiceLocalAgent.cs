using Domain;
using Domain.Services.Modes;

namespace GateApp
{
    // TODO: put events/alarms etc.
    public class ModeServiceLocalAgent : IModeCommandService
    {
        private readonly ModeManager modeMgr;
        private readonly IContextRepository contextRepo;        

        public ModeServiceLocalAgent(ModeManager modeMgr, IContextRepository contextRepo)
        {
            this.modeMgr = modeMgr;
            this.contextRepo = contextRepo;
        }

        public Mode CurMode => modeMgr.CurMode;

        public async Task SwitchToMaintenance()
        {
            if (modeMgr.CurMode == Mode.Maintenance)
                return;

            await modeMgr.SwitchToMaintenance();            
        }

        public async Task SwitchOutMaintenance()
        {
            if (modeMgr.CurMode != Mode.Maintenance)
                return;            
 
            await modeMgr.SwitchOutMaintenance();
        }

        public void ChangeMode(OpMode mode, TimeSpan timeout)
        {
            if (modeMgr.ModeDemanded == mode)
                return ;

            modeMgr.ModeDemanded = mode;
            contextRepo.SaveMode(mode);
            
            //return false;
        }
    }
}