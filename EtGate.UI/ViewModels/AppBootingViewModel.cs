using GateApp;
using System;

namespace EtGate.UI.ViewModels
{
    public class AppBootingViewModel : ModeViewModel, IDisposable
    {
        public AppBootingViewModel(IModeService modeService) : base(modeService)
        {
        }

        public void Dispose()
        {
            
        }
    }
}
