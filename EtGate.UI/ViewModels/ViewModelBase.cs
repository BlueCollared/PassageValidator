using GateApp;
using ReactiveUI;

namespace EtGate.UI.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
    }

    public class ModeViewModel : ViewModelBase
    {
        private readonly IModeService modeService;

        public ModeViewModel(IModeService modeService)
        {
            this.modeService = modeService;
        }
        public void MaintenaceRequested()
        {
            modeService.SwitchToMaintenance();
        }
    }
}
