using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EtGate.UI.ViewModels;

namespace EtGate.UI.Views;

public partial class MaintenanceView : UserControl
{
    public MaintenanceView()
    {
        InitializeComponent();

        this.DataContextChanged += OnDataContextChanged;
        //var viewModel = (MaintenanceViewModel)DataContext;
        //var x = host;
        //ViewModelBehavior.SetInitAction(host, viewModel.Init);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnDataContextChanged(object sender, System.EventArgs e)
    {
        if (this.DataContext is MaintenanceViewModel viewModel)
        {
            var hostControl = this.FindControl<ContentControl>("host");
            viewModel.Init(hostControl);
        }
    }
}