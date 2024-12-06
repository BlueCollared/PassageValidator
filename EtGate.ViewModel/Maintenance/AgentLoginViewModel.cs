using EtGate.Domain.Services;
using ReactiveUI;

using System.Windows.Input;

namespace EtGate.UI.ViewModels.Maintenance;

public class AgentLoginViewModel : MaintainenaceViewModelBase
{
    private readonly ILoginService loginService;
    private readonly INavigationService navService;

    // TODO: provision for raising the event.
    public AgentLoginViewModel(ILoginService loginService, INavigationService navService) : base(navService)
    {
        this.loginService = loginService;
        this.navService = navService;
        LoginCommand = ReactiveCommand.Create(Login, outputScheduler: RxApp.MainThreadScheduler);
    }

    public ICommand LoginCommand { get; private set; }

    public override void Dispose()
    {
        IsDisposed = true;
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
