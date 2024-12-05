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

    public MaintainenaceViewModelBase CurrentViewModel => _viewModelStack.Any() ? _viewModelStack.Peek() : null;    

    public void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase
    {
        CurrentViewModel?.Dispose();        

        TViewModel viewModel = EstablishVM<TViewModel>(viewModelFactory);

        _viewModelStack.Push(viewModel);
        _navigationEventManager.RaiseNavigated(viewModel);
    }

    public void GoBack()
    {
        if (!_viewModelStack.Any())
        {
            modeService.SwitchOutMaintenance();
            return;
        }

        var vmTop = _viewModelStack.Pop();
        vmTop.Dispose();

        if (!_viewModelStack.Any())
        {
            modeService.SwitchOutMaintenance();
            return;
        }

        vmTop = _viewModelStack.Pop();

        if (vmTop.GetType() == typeof(AgentLoginViewModel)
            || vmTop.GetType().IsSubclassOf(typeof(AgentLoginViewModel)))
        {
            _viewModelStack.Clear();
            modeService.SwitchOutMaintenance();
        }
        else
        {
            MaintainenaceViewModelBase viewModel = CallEstablishVM(vmTop, viewModelFactory);            
            _viewModelStack.Push(viewModel);
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
        CallEstablishVM(MaintainenaceViewModelBase x, 
        Func<Type, MaintainenaceViewModelBase> viewModelFactory        
        )
    {
        Type viewModelType = x.GetType();
        return viewModelFactory(viewModelType);        
    }

    public Stack<MaintainenaceViewModelBase> ViewModelStack => new Stack<MaintainenaceViewModelBase>(_viewModelStack);

    private readonly Stack<MaintainenaceViewModelBase> _viewModelStack = new();
    private readonly Func<Type, MaintainenaceViewModelBase> viewModelFactory;
    private readonly INavigationEventManager _navigationEventManager;    

    private readonly IModeManager modeService;    
}