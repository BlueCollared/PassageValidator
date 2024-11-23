using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace MaintenanceMenu
{
    public partial class App : Application
    {
        public void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)            
                desktop.MainWindow = new MainWindow();
            
            base.OnFrameworkInitializationCompleted();
        }
    }
}