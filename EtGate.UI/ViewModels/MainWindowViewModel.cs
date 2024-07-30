namespace EtGate.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase CurrentModeViewModel { get; set; } = new AppBootingViewModel();
//#pragma warning disable CA1822 // Mark members as static
//        public string Greeting => "Welcome to Avalonia!";
//#pragma warning restore CA1822 // Mark members as static
    }
}
