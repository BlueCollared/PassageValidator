using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EtGate.UI;


public interface INavigationService
{
    ContentControl host { get; set; }
    void NavigateTo<TViewModel>() where TViewModel : class;
}

public class MaintenanceNavigationService : INavigationService
{
    //private readonly
    public
     IServiceProvider _serviceProvider { get; set; }

    public MaintenanceNavigationService(
        IServiceProvider serviceProvider
        )
    {
        _serviceProvider = serviceProvider;
    }

    public ContentControl host { get; set; }

    public void NavigateTo<TViewModel>() where TViewModel : class
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        var viewType = typeof(TViewModel).Name.Replace("ViewModel", "View");
        var view = (Control)Activator.CreateInstance(Type.GetType($"EtGate.UI.Views.Maintenance.{viewType}"));
        view.DataContext = viewModel;
        host.Content = view;
    }
}
