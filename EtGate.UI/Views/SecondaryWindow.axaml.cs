using Avalonia.Controls;
using Avalonia.Input;
using EtGate.UI.ViewModels;

namespace EtGate.UI.Views;

public partial class SecondaryWindow : Window
{
    public SecondaryWindow()
    {
        InitializeComponent();
        this.KeyDown += OnKeyDown;
        if (!Design.IsDesignMode)
            PositionWindowOnSecondaryScreen();
    }

    private void PositionWindowOnSecondaryScreen()
    {
        var screens = Screens.All;
        if (screens.Count < 2)
            return;
        var secondaryScreen = screens[1];
        if (secondaryScreen != null)        
            Position = secondaryScreen.WorkingArea.TopLeft;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.C && e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            // Ctrl+C
            ((ModeViewModel)(host.Content)).MaintenaceRequested();
            e.Handled = true; // Mark the event as handled if needed
        }
    }
}