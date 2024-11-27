using Domain.Services.Modes;

namespace EtGate.UI.ViewModels;

public class MaintenanceViewModelPassive : ModeViewModel
{
    public MaintenanceViewModelPassive(IModeManager modeService) : base(modeService)
    {}
}