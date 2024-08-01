using Domain;
using Domain.Services.InService;
using GateApp;
using ReactiveUI;

namespace EtGate.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IModeService modeService;
        private readonly IInServiceMgrFactory inServiceMgrFactory;

        public MainWindowViewModel(IModeService modeService, IInServiceMgrFactory inServiceMgrFactory)
        {
            this.modeService = modeService;
            this.inServiceMgrFactory = inServiceMgrFactory;
            this.modeService.EquipmentModeObservable.Subscribe(x => ModeChanged(x));
        }

        private void ModeChanged(Mode x)
        {
            if (curMode == x)
                return;
            
            switch(x)
            {
                case Mode.AppBooting:
                    CurrentModeViewModel = new AppBootingViewModel(modeService);
                    break;
                case Mode.OOO:
                    CurrentModeViewModel = new OOOViewModel(modeService);
                    break;
                case Mode.Emergency:
                    CurrentModeViewModel = new EmergencyViewModel(modeService);
                    break;
                case Mode.InService:
                    CurrentModeViewModel = new InServiceViewModel(inServiceMgrFactory.Create(), true, modeService);
                    break;
                case Mode.OOS:
                    CurrentModeViewModel = new OOSViewModel(modeService);
                    break;
                case Mode.Maintenance:
                    CurrentModeViewModel = new MaintenanceViewModel(modeService);
                    break;
            }
            curMode = x;
            this.RaisePropertyChanged(nameof(CurrentModeViewModel));
            //PropertyChanged(nameof(CurrentModeViewModel));
        }

        public ModeViewModel CurrentModeViewModel { get; set; }
        private Mode? curMode = null;
        //= new AppBootingViewModel();
        //#pragma warning disable CA1822 // Mark members as static
        //        public string Greeting => "Welcome to Avalonia!";
        //#pragma warning restore CA1822 // Mark members as static
    }
}
