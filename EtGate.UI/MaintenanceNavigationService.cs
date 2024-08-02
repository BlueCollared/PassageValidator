using Avalonia.Controls;
using System;

namespace EtGate.UI;


public interface INavigationService
{
    void NavigateTo<TViewModel>(ContentControl host) where TViewModel : class;
}

public class MaintenanceNavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    public MaintenanceNavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<TViewModel>(ContentControl host) where TViewModel : class
    {        
        //var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        //var viewType = typeof(TViewModel).Name.Replace("ViewModel", "View");
        //var view = (Control)Activator.CreateInstance(Type.GetType($"YourNamespace.{viewType}"));
        //view.DataContext = viewModel;
        //host.Content = view;
    }
}