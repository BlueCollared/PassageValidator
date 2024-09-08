using Domain;
using Domain.Services.InService;

namespace GateApp
{

    public interface IModeCommandService
    {
        //Mode CurMode { get; }
        

        bool ChangeMode(OpMode mode, TimeSpan timeout);
        Task SwitchOutMaintenance();
        Task SwitchToMaintenance();
        ISubModeMgr curModeMgr { get; }
    }
}