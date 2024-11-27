using Domain.Services.Modes;

namespace EtGate.UI.ViewModels
{
    public class EmergencyViewModel : ModeViewModel
    {
        public EmergencyViewModel(IModeManager modeService) : base(modeService)
        {
        }
    }
}
