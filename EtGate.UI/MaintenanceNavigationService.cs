using Domain.Services.Modes;
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
        IModeManager modeService
        )
    {
        this.viewModelFactory = viewModelFactory;
        _navigationEventManager = navigationEventManager;
        this.modeService = modeService;
    }   

    public MaintainenaceViewModelBase CurrentViewModel ;//=> _viewModelStack.Any() ? _viewModelStack.Peek() : null;    

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
            modeService.SwitchOutMaintenance();
            return;
        }

        var _ = _viewModelStack.Pop();
        CurrentViewModel?.Dispose();
        CurrentViewModel = null;

        if (!_viewModelStack.Any())
        {
            modeService.SwitchOutMaintenance();
            return;
        }

        var vmTop = _viewModelStack.Peek();

        if (vmTop.GetType() == typeof(AgentLoginViewModel)
            || vmTop.GetType().IsSubclassOf(typeof(AgentLoginViewModel)))
        {
            _viewModelStack.Clear();
            modeService.SwitchOutMaintenance();
        }
        else
        {
            MaintainenaceViewModelBase viewModel = CallEstablishVM(vmTop, viewModelFactory);
            _navigationEventManager.RaiseNavigated(viewModel);            
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

    public Stack<Type> ViewModelStack => new Stack<Type>(_viewModelStack);

    private readonly Stack<Type> _viewModelStack = new();
    private readonly Func<Type, MaintainenaceViewModelBase> viewModelFactory;
    private readonly INavigationEventManager _navigationEventManager;    

    private readonly IModeManager modeService;    
}