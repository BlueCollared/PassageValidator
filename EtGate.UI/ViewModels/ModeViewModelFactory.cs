using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core.Message;
using EtGate.Domain;
using EtGate.Domain.Peripherals.Qr;
using System;

namespace EtGate.UI.ViewModels
{
    public class ModeViewModelFactory : IModeViewModelFactory
    {
        private readonly IModeManager modeService;
        private readonly INavigationService maintenanceNavigationService;
        private readonly DeviceStatusSubscriber<QrReaderStatus> qr;

        public ModeViewModelFactory(IModeManager modeService,
            INavigationService maintenanceNavigationService,
            DeviceStatusSubscriber<QrReaderStatus> qr = null)
        {
            this.modeService = modeService;
            this.maintenanceNavigationService = maintenanceNavigationService;
            this.qr = qr;
        }

        public ModeViewModel Create(Mode mode, global::Domain.Services.InService.ISubModeMgr subModeMgr, bool bPrimary, bool bEntry)
        {
            return mode switch
            {
                Mode.AppBooting => new AppBootingViewModel(modeService, qr),
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
