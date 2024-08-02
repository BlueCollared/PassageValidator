using Avalonia.Controls;
using GateApp;

namespace EtGate.UI.ViewModels;

public class MaintenanceViewModel : ModeViewModel//, IContentControlHost
{
    private readonly INavigationService navigationService;

    public MaintenanceViewModel(IModeService modeService, INavigationService navigationService) : base(modeService)
    {
        this.navigationService = navigationService;
    }

    //public ContentControl ContentControl { get; set; }

    public void Init(ContentControl host)
    {
        //navigationService.Host = host;
        navigationService.NavigateTo<AgentLoginViewModel>();
    }
}

//public interface IContentControlHost
//{
//    ContentControl ContentControl { get; set; }
//    void Init(ContentControl host);
//}