using Avalonia.Controls;
using EtGate.QrReader.Proxy;
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
        IModeService modeService
        )
    {
        this.viewModelFactory = viewModelFactory;        
        this.modeService = modeService;
    }

    public ContentControl host { get; set; }

    public MaintainenaceViewModelBase CurrentViewModel => _viewModelStack.Any() ? _viewModelStack.Peek() : null;

    public void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase
    {
        CurrentViewModel?.Dispose();        

        (TViewModel viewModel, UserControl view) = EstablishVM<TViewModel>(viewModelFactory);
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

        var vmTop = _viewModelStack.Peek();
        if (vmTop.GetType() == typeof(AgentLoginViewModel)
            || vmTop.GetType().IsSubclassOf(typeof(AgentLoginViewModel)))
        {
            _viewModelStack.Clear();
            modeService.SwitchOutMaintenance();
        }
        else
        {
            (MaintainenaceViewModelBase viewModel, UserControl view) = CallEstablishVM(vmTop, viewModelFactory);
            view.DataContext = viewModel;
            host.Content = view;
        }
    }

    private static (TViewModel viewModel, UserControl view) EstablishVM<TViewModel>(Func<Type, MaintainenaceViewModelBase> viewModelFactory) 
        where TViewModel : MaintainenaceViewModelBase
    {
        Type viewModelType = typeof(TViewModel);        
        MaintainenaceViewModelBase viewModelBase = viewModelFactory(viewModelType);
        TViewModel viewModel = (TViewModel)viewModelBase;
        
        var viewType = typeof(TViewModel).Name.Replace("ViewModel", "View");
        var view = (UserControl)Activator.CreateInstance(Type.GetType($"EtGate.UI.Views.Maintenance.{viewType}"));
        return (viewModel, view);
    }

    private static (MaintainenaceViewModelBase, UserControl view) CallEstablishVM(MaintainenaceViewModelBase x, Func<Type, MaintainenaceViewModelBase> viewModelFactory)
    {
        Type viewModelType = x.GetType();
        MaintainenaceViewModelBase viewModelBase = viewModelFactory(viewModelType);

        var viewType = viewModelBase.GetType().Name.Replace("ViewModel", "View");
        var view = (UserControl)Activator.CreateInstance(Type.GetType($"EtGate.UI.Views.Maintenance.{viewType}"));

        return (viewModelBase, view);
    }
    private readonly Stack<MaintainenaceViewModelBase> _viewModelStack = new();
    private readonly Func<Type, MaintainenaceViewModelBase> viewModelFactory;
    private readonly IModeService modeService;    
}