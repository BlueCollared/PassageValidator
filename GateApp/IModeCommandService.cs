using EtGate.Domain;

namespace GateApp
{

    public interface IModeCommandService
    {
        //Mode CurMode { get; }
        

        void ChangeMode(OpMode mode, TimeSpan timeout);
        Task SwitchOutMaintenance();
        Task SwitchToMaintenance();
        //ISubModeMgr curModeMgr { get; }
    }
}