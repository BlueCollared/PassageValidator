using Avalonia.Controls;
using EtGate.UI.ViewModels.Maintenance;
using GateApp;

namespace EtGate.UI.ViewModels;

public class MaintenanceViewModel : ModeViewModel//, IContentControlHost
{
    private readonly INavigationService _navigationService;

    public MaintenanceViewModel(IModeService modeService, INavigationService navigationService) : base(modeService)
    {
        _navigationService = navigationService;
    }

    //public ContentControl ContentControl { get; set; }

    public void Init(ContentControl host)
    {
        _navigationService.host = host;
        _navigationService.NavigateTo<AgentLoginViewModel>();
    }
}

//public interface IContentControlHost
//{
//    ContentControl ContentControl { get; set; }
//    void Init(ContentControl host);
//}