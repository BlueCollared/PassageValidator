using EtGate.Domain.Services;
using ReactiveUI;

//using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace EtGate.UI.ViewModels.Maintenance;

public class AgentLoginViewModel : MaintainenaceViewModelBase
{
    private readonly LoginService loginService;
    private readonly INavigationService navService;

    // TODO: provision for raising the event.
    public AgentLoginViewModel(LoginService loginService, INavigationService navService) : base(navService)
    {
        this.loginService = loginService;
        this.navService = navService;
        LoginCommand = ReactiveCommand.Create(Login);
    }

    public ICommand LoginCommand { get; private set; }

    public override void Dispose()
    {        
    }

    private void Login(/*string userId, string passwd*/)
    {
        //Agent? loginResult = loginService.Login(userId, passwd).Result;
        //if (loginResult != null)
        {
            navService.NavigateTo<MaintenanceMenuViewModel>();
        }
    }
}
