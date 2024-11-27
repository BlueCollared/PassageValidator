using Domain.Services.Modes;
using EtGate.UI.ViewModels.Maintenance;

namespace EtGate.UI.ViewModels;

public class MaintenanceViewModel : ModeViewModel
{
    private readonly INavigationService _navigationService;

    public MaintenanceViewModel(IModeManager modeService, INavigationService navigationService) : base(modeService)
    {
        _navigationService = navigationService;        
    }   

    public void Init()
    {        
        _navigationService.NavigateTo<AgentLoginViewModel>();
    }
}