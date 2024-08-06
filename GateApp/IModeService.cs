using Domain;
using Domain.Services.InService;

namespace GateApp
{
    public interface IModeService
    {
        //Mode CurMode { get; }
        IObservable<Mode> EquipmentModeObservable { get; }

        bool ChangeMode(OpMode mode, TimeSpan timeout);
        void SwitchOutMaintenance();
        void SwitchToMaintenance();
        ISubModeMgr curModeMgr { get; }
    }
}