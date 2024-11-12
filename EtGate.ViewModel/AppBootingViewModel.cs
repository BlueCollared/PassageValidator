using GateApp;
using System;

namespace EtGate.UI.ViewModels
{
    public class AppBootingViewModel : ModeViewModel, IDisposable
    {
        public AppBootingViewModel(IModeCommandService modeService) : base(modeService)
        {
        }

        public void Dispose()
        {
            
        }
    }
}
