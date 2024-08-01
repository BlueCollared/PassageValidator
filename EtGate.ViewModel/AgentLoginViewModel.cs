using EtGate.Domain;
using EtGate.Domain.Services;
using System.Windows.Input;

namespace EtGate.UI.ViewModels;

public class AgentLoginViewModel
{
    private readonly LoginService loginService;
    private readonly INavigationService navService;

    // TODO: provision for raising the event.
    public AgentLoginViewModel(LoginService loginService, INavigationService navService)
    {
        this.loginService = loginService;
        this.navService = navService;
    }

    public ICommand LoginCommand { get; }

    private void Login(string userId, string passwd)
    {
        Agent? loginResult = loginService.Login(userId, passwd).Result;
        if (loginResult != null)
        {
            navService.NavigateTo<MaintenanceMenuViewModel>();
        }
    }
}
