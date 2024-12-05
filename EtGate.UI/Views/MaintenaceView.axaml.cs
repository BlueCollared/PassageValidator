using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EtGate.UI.ViewModels;

namespace EtGate.UI.Views;

public partial class MaintenanceView : UserControl
{
    private readonly INavigationService _navigationService;
    private readonly IViewFactory _viewLocator;

    public MaintenanceView() //: this(App.Container?.Resolve<INavigationEventManager>(), App.Container?.Resolve<IViewFactory>())
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
