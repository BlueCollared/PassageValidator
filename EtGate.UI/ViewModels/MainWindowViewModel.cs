using Domain;
using GateApp;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace EtGate.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IModeService modeService;
        private readonly INavigationService maintenanceNavigationService; // TODO: we are using a singleton of it, while we should be creating a new instance for every session
        private readonly bool bPrimary;

        public MainWindowViewModel(IModeService modeService,
            INavigationService maintenanceNavigationService,
            bool bPrimary)
        {
            this.modeService = modeService;
            this.maintenanceNavigationService = maintenanceNavigationService;
            this.bPrimary = bPrimary;
            this.modeService.EquipmentModeObservable.Subscribe(x => ModeChanged(x));
        }

        private void ModeChanged(Mode x)
        {
            if (curMode == x)
                return;
            if (CurrentModeViewModel is IDisposable y)
                y.Dispose();
            switch (x)
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
                    CurrentModeViewModel = new InServiceViewModel(                        
                        true, modeService);
                    break;
                case Mode.OOS:
                    CurrentModeViewModel = new OOSViewModel(modeService);
                    break;
                case Mode.Maintenance:
                    if (bPrimary)
                        CurrentModeViewModel = new MaintenanceViewModel(modeService, maintenanceNavigationService);
                    else
                        CurrentModeViewModel = new MaintenanceViewModelPassive(modeService);
                    break;
            }
            curMode = x;
        }

        private ModeViewModel _currentModeViewModel;
        public ModeViewModel CurrentModeViewModel
        {
            get => _currentModeViewModel;
            set
            {
                if (_currentModeViewModel != value)
                {
                    _currentModeViewModel = value;
                    this.RaisePropertyChanged(nameof(CurrentModeViewModel));
                }
            }
        }
        
        private Mode? curMode = null;
    }
}
