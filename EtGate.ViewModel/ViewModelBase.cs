﻿using Domain.Services.Modes;
using ReactiveUI;
using System.Windows.Input;

namespace EtGate.UI.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject
    {
    }

    public abstract class MaintainenaceViewModelBase : ViewModelBase, IDisposable
    {
        private readonly INavigationService navigationService;

        public ICommand GoBackCommand { get; protected set; }

        protected MaintainenaceViewModelBase(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            GoBackCommand = ReactiveCommand.Create(() => navigationService.GoBack());
        }

        public bool IsDisposed { get; protected set; } = false;
        public abstract void Dispose();
    }

    public class ModeViewModel : ViewModelBase
    {
        private readonly IModeManager modeService;

        public ModeViewModel(IModeManager modeService)
        {
            this.modeService = modeService;
        }
        public void MaintenaceRequested()
        {
            modeService.SwitchToMaintenance();
        }
    }
}
