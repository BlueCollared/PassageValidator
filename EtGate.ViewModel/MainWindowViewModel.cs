using Domain;
using GateApp;

namespace EtGate.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IModeService modeService;

        public MainWindowViewModel(IModeService modeService)
        {
            this.modeService = modeService;
            this.modeService.EquipmentModeObservable.Subscribe(x => ModeChanged(x));
        }

        private void ModeChanged(Mode x)
        {
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
            }
        }

        public ViewModelBase CurrentModeViewModel { get; set; }        
        //= new AppBootingViewModel();
        //#pragma warning disable CA1822 // Mark members as static
        //        public string Greeting => "Welcome to Avalonia!";
        //#pragma warning restore CA1822 // Mark members as static
    }
}
