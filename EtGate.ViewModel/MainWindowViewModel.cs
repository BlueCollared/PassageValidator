using Domain;
using Domain.Services.InService;
using EtGate.ViewModel;
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
                    CurrentModeViewModel = new AppBootingViewModel();
                    break;
                case Mode.OOO:
                    CurrentModeViewModel = new OOOViewModel();
                    break;
                case Mode.Emergency:
                    CurrentModeViewModel = new EmergencyViewModel();
                    break;
                case Mode.InService:
                    CurrentModeViewModel = new InServiceViewModel(inServiceMgrFactory.Create(), true);
                    break;
                case Mode.OOS:
                    CurrentModeViewModel = new OOSViewModel();
                    break;
                case Mode.Maintenance:
                    CurrentModeViewModel = new MaintenanceViewModel();
                    break;
            }
            curMode = x;
            this.RaisePropertyChanged(nameof(CurrentModeViewModel));
            //PropertyChanged(nameof(CurrentModeViewModel));
        }

        public ViewModelBase CurrentModeViewModel { get; set; }
        private Mode? curMode = null;
        //= new AppBootingViewModel();
        //#pragma warning disable CA1822 // Mark members as static
        //        public string Greeting => "Welcome to Avalonia!";
        //#pragma warning restore CA1822 // Mark members as static
    }
}
