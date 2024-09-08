using Avalonia.Controls;
using EtGate.UI.ViewModels;
using EtGate.UI.ViewModels.Maintenance;
using GateApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EtGate.UI;

public interface INavigationService
{
    ContentControl host { get; set; }
    void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase;
    void GoBack();
}

public class MaintenanceNavigationService : INavigationService
{
    public MaintenanceNavigationService(        
        Func<Type, MaintainenaceViewModelBase> viewModelFactory,
        IViewFactory viewFactory,
        IModeCommandService modeService
        )
    {
        this.viewModelFactory = viewModelFactory;
        this.viewFactory = viewFactory;
        this.modeService = modeService;
    }

    public ContentControl host { get; set; }

    public MaintainenaceViewModelBase CurrentViewModel => _viewModelStack.Any() ? _viewModelStack.Peek() : null;

    public void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase
    {
        CurrentViewModel?.Dispose();        

        (TViewModel viewModel, UserControl view) = EstablishVM<TViewModel>(viewModelFactory, viewFactory);
        view.DataContext = viewModel;
        host.Content = view;

        _viewModelStack.Push(viewModel);
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
            (MaintainenaceViewModelBase viewModel, UserControl view) = CallEstablishVM(vmTop, viewModelFactory, viewFactory);
            view.DataContext = viewModel;
            host.Content = view;
            _viewModelStack.Push(viewModel);
        }
    }

    private static (TViewModel viewModel, UserControl view) EstablishVM<TViewModel>(Func<Type, MaintainenaceViewModelBase> viewModelFactory,
        IViewFactory viewFactory) 
        where TViewModel : MaintainenaceViewModelBase
    {
        Type viewModelType = typeof(TViewModel);        
        MaintainenaceViewModelBase viewModelBase = viewModelFactory(viewModelType);
        TViewModel viewModel = (TViewModel)viewModelBase;
        
        return (viewModel, viewFactory.Create(typeof(TViewModel)));
    }

    private static (MaintainenaceViewModelBase, UserControl view) 
        CallEstablishVM(MaintainenaceViewModelBase x, 
        Func<Type, MaintainenaceViewModelBase> viewModelFactory, 
        IViewFactory viewFactory)
    {
        Type viewModelType = x.GetType();
        MaintainenaceViewModelBase viewModelBase = viewModelFactory(viewModelType);
        
        return (viewModelBase, viewFactory.Create(viewModelType));
    }

    public Stack<MaintainenaceViewModelBase> ViewModelStack => new Stack<MaintainenaceViewModelBase>(_viewModelStack);

    private readonly Stack<MaintainenaceViewModelBase> _viewModelStack = new();
    private readonly Func<Type, MaintainenaceViewModelBase> viewModelFactory;
    private readonly IViewFactory viewFactory;
    private readonly IModeCommandService modeService;    
}