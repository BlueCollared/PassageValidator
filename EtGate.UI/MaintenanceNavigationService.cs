using EtGate.Domain.Services;
using EtGate.UI.ViewModels;
using EtGate.UI.ViewModels.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtGate.UI;

public class MaintenanceNavigationService : INavigationService
{
    public MaintenanceNavigationService(
        Func<Type, MaintainenaceViewModelBase> viewModelFactory,
         INavigationEventManager navigationEventManager,
        //IModeManager modeService
        ISessionManager sessionManager
        )
    {
        this.viewModelFactory = viewModelFactory;
        _navigationEventManager = navigationEventManager;
        this.sessionManager = sessionManager;
        //this.modeService = modeService;
    }

    public MaintainenaceViewModelBase CurrentViewModel { get; private set; }

    public void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase
    {
        CurrentViewModel?.Dispose();

        CurrentViewModel = EstablishVM<TViewModel>(viewModelFactory);

        _viewModelStack.Push(CurrentViewModel.GetType());
        _navigationEventManager.RaiseNavigated(CurrentViewModel);
    }

    public void GoBack()
    {
        if (!_viewModelStack.Any())
        {
//            modeService.SwitchOutMaintenance();
sessionManager.AgentLoggedOut();
            return;
        }

        var _ = _viewModelStack.Pop();
        CurrentViewModel?.Dispose();
        CurrentViewModel = null;

        if (!_viewModelStack.Any())
        {
            sessionManager.AgentLoggedOut();
            //modeService.SwitchOutMaintenance();
            return;
        }

        var vmTop = _viewModelStack.Peek();

        if (vmTop == typeof(AgentLoginViewModel)
            || vmTop.IsSubclassOf(typeof(AgentLoginViewModel)))
        {
            _viewModelStack.Clear();
            sessionManager.AgentLoggedOut();
            //modeService.SwitchOutMaintenance();
        }
        else
        {
            CurrentViewModel = CallEstablishVM(vmTop, viewModelFactory);
            _navigationEventManager.RaiseNavigated(CurrentViewModel);
        }
    }

    private static TViewModel EstablishVM<TViewModel>(Func<Type, MaintainenaceViewModelBase> viewModelFactory)
        where TViewModel : MaintainenaceViewModelBase
    {
        Type viewModelType = typeof(TViewModel);
        MaintainenaceViewModelBase viewModelBase = viewModelFactory(viewModelType);
        TViewModel viewModel = (TViewModel)viewModelBase;

        return viewModel;
    }

    private static MaintainenaceViewModelBase
        CallEstablishVM(Type viewModelType,
        Func<Type, MaintainenaceViewModelBase> viewModelFactory
        )
    {
        //Type viewModelType = x.GetType();
        return viewModelFactory(viewModelType);
    }

    public Stack<Type> ViewModelStackCopy => new Stack<Type>(_viewModelStack);

    private readonly Stack<Type> _viewModelStack = new();
    private readonly Func<Type, MaintainenaceViewModelBase> viewModelFactory;
    private readonly INavigationEventManager _navigationEventManager;
    private readonly ISessionManager sessionManager;
    //private readonly IModeManager modeService;    
}