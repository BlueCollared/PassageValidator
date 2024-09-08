using Avalonia.Controls;
using EtGate.UI.ViewModels.Maintenance;
using GateApp;

namespace EtGate.UI.ViewModels;

public class MaintenanceViewModel : ModeViewModel
{
    private readonly INavigationService _navigationService;

    public MaintenanceViewModel(IModeCommandService modeService, INavigationService navigationService) : base(modeService)
    {
        _navigationService = navigationService;
    }   

    public void Init(ContentControl host)
    {
        _navigationService.host = host;
        _navigationService.NavigateTo<AgentLoginViewModel>();
    }
}