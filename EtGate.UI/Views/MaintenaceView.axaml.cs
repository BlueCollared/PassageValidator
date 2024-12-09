using Autofac;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using EtGate.UI.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using System.Reactive.Disposables;

namespace EtGate.UI.Views;

public partial class MaintenanceView : ReactiveUserControl<MaintenanceViewModel>
{
    private readonly INavigationService _navigationService;
    private readonly IViewFactory _viewLocator;

    public MaintenanceView() : this(DepBuilder.Container?.Resolve<INavigationEventManager>(), DepBuilder.Container?.Resolve<IViewFactory>())
    { }

    public MaintenanceView(INavigationEventManager? navigationEventManager, IViewFactory? viewLocator)
    {
        InitializeComponent();

        if (Design.IsDesignMode)
            return;

        _viewLocator = viewLocator;
        navigationEventManager.Navigated += OnNavigated;

        this.WhenActivated(disposables =>
        {
            // Handle the Interaction
            ViewModel?.ShowShiftTimedOutDialog.RegisterHandler(async interaction =>
            {
                var result = await MessageBoxManager.GetMessageBoxStandard("Shift Timeout", interaction.Input,
                        ButtonEnum.YesNo)
                    .ShowAsPopupAsync(this);

                // Pass the result back to the ViewModel
                interaction.SetOutput(result == ButtonResult.Yes);
            }).DisposeWith(disposables);
        });
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