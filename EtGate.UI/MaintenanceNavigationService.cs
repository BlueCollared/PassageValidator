using Avalonia.Controls;
using EtGate.UI.ViewModels;
using EtGate.UI.ViewModels.Maintenance;
using GateApp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EtGate.UI;

public interface INavigationService
{
    ContentControl host { get; set; }
    void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase;
    void GoBack();
}

public class MaintenanceNavigationService : INavigationService
{
    //private readonly
    public
     IServiceProvider _serviceProvider { get; set; }

    public MaintenanceNavigationService(
        IServiceProvider serviceProvider,
        IModeService modeService
        )
    {
        _serviceProvider = serviceProvider;
        this.modeService = modeService;
    }

    public ContentControl host { get; set; }

    public void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase
    {
        if (_viewModelStack.Any())
        {
            var currentViewModel = _viewModelStack.Peek();
            currentViewModel.Dispose();
        }

        TViewModel viewModel = EstablishVM<TViewModel>();

        _viewModelStack.Push(viewModel);
    }

    private TViewModel EstablishVM<TViewModel>() where TViewModel : MaintainenaceViewModelBase
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        var viewType = typeof(TViewModel).Name.Replace("ViewModel", "View");
        var view = (Control)Activator.CreateInstance(Type.GetType($"EtGate.UI.Views.Maintenance.{viewType}"));
        view.DataContext = viewModel;
        host.Content = view;
        return viewModel;
    }

    private MaintainenaceViewModelBase CallEstablishVM(MaintainenaceViewModelBase x)
    {        
        Type xType = x.GetType();
        
        Type baseType = typeof(MaintainenaceViewModelBase);

        // Ensure that x's type derives from MaintainenaceViewModelBase
        if (baseType.IsAssignableFrom(xType))
        {            
            MethodInfo method = typeof(MaintenanceNavigationService).GetMethod(nameof(EstablishVM), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo genericMethod = method.MakeGenericMethod(xType);
            
            var result = genericMethod.Invoke(this, null);
            
            return (MaintainenaceViewModelBase)result;
        }
        else
            return null;
    }
    private readonly Stack<MaintainenaceViewModelBase> _viewModelStack = new Stack<MaintainenaceViewModelBase>();
    private readonly IModeService modeService;

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
            CallEstablishVM(vmTop);
        }        
    }
}
