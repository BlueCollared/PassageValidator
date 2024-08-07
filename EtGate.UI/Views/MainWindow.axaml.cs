using Avalonia.Controls;
using Avalonia.Input;
using EtGate.Domain.Services;
using EtGate.UI.ViewModels;
//using Key = Avalonia.Remote.Protocol.Input.Key;
using Key = Avalonia.Input.Key;

namespace EtGate.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += OnKeyDown;
            if (!Design.IsDesignMode)
                PositionWindowOnPrimaryScreen();
        }

        public LoginService loginService { private get; set; }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && e.KeyModifiers.HasFlag(KeyModifiers.Control))
            {
                // Ctrl+C
                ((ModeViewModel)(host.Content)).MaintenaceRequested();
                e.Handled = true; // Mark the event as handled if needed
            }
        }

        private void PositionWindowOnPrimaryScreen()
        {
            var screens = Screens.All;
            if (screens.Count < 1)
                return;
            var primaryScreen = screens[0];//.FirstOrDefault(s => s.IsPrimary);
            if (primaryScreen != null)
            {
                Position = primaryScreen.WorkingArea.TopLeft;
            }
        }
    }
}