using Domain.Services.InService;
using EtGate.Domain;
using GateApp;
using System;

namespace EtGate.UI.ViewModels
{
    public class ModeViewModelFactory : IModeViewModelFactory
    {
        private readonly IModeCommandService modeService;
        private readonly INavigationService maintenanceNavigationService;

        public ModeViewModelFactory(IModeCommandService modeService, INavigationService maintenanceNavigationService)
        {
            this.modeService = modeService;
            this.maintenanceNavigationService = maintenanceNavigationService;
        }

        public ModeViewModel Create(Mode mode, global::Domain.Services.InService.ISubModeMgr subModeMgr, bool bPrimary, bool bEntry)
        {
            return mode switch
            {
                Mode.AppBooting => new AppBootingViewModel(modeService),
                Mode.InService => new InServiceViewModel(bEntry, modeService, (IInServiceMgr)subModeMgr),
                Mode.OOS => new OOSViewModel(modeService),
                Mode.Emergency => new EmergencyViewModel(modeService),
                Mode.OOO => new OOOViewModel(modeService),
                Mode.Maintenance => bPrimary ? new MaintenanceViewModel(modeService, maintenanceNavigationService) : new MaintenanceViewModelPassive(modeService),
                _ => throw new ArgumentException("Unknown mode")
            };
        }        
    }
}
