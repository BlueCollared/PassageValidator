using Autofac;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EtGate.UI.ViewModels;

namespace EtGate.UI.Views;

public partial class MaintenanceView : UserControl
{
    private readonly INavigationService _navigationService;
    private readonly IViewFactory _viewLocator;

    public MaintenanceView() : this(DepBuilder.Container?.Resolve<INavigationEventManager>(), DepBuilder.Container?.Resolve<IViewFactory>())
    {}
    
    public MaintenanceView(INavigationEventManager? navigationEventManager, IViewFactory? viewLocator)
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;

        _viewLocator = viewLocator;
        navigationEventManager.Navigated += OnNavigated;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void OnNavigated(MaintainenaceViewModelBase viewModel)
    {
        var hostControl = this.FindControl<ContentControl>("host");
        var view = _viewLocator.Create(viewModel.GetType());
        view.DataContext = viewModel;
        hostControl.Content = view;
    }
}

//public static class App
//{
//    // a back-door entry to the DI container. To be used as service locator but only in exceptional scenarios
//    public static IContainer Container { get; set; }
//}