using GateApp;

namespace EtGate.UI.ViewModels
{
    public class AppBootingViewModel : ModeViewModel, IDisposable
    {
        public AppBootingViewModel(IModeCommandService modeService) : base(modeService)
        {
        }
        public string QrRdrStatus { get; set; }
        public void Dispose()
        {
            
        }
    }
}
