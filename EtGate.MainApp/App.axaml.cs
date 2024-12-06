using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EtGate.DependencyInjection;
using EtGate.UI;
using EtGate.UI.ViewModels;
using EtGate.UI.Views;

namespace EtGate.MainApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        const string PrimaryEntry = nameof(PrimaryEntry);
        const string PrimaryExit = nameof(PrimaryExit);
        const string SecondaryExit = nameof(SecondaryExit);
        const string SecondaryEntry = nameof(SecondaryEntry);

        const string bPrimary = "bPrimary";
        const string bEntry = "bEntry";


        public
            //static 
            Autofac.IContainer? Container
        { get; private set; }

        public override void OnFrameworkInitializationCompleted()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new GateModule(GateDepNature.Real));
            builder.RegisterModule(new QrModule(QrDepNature.Real));
            builder.RegisterModule(new ValidationModule(ValidationDepNature.Real));
            builder.RegisterModule(new ModeModule());
            builder.RegisterModule(new TemporaryModule());

            DepBuilder.Do(builder);

            DepBuilder.Container = Container = builder.Build();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Container.ResolveNamed<MainWindowViewModel>(PrimaryEntry)
                };

                var secondaryWindow = new SecondaryWindow
                {
                    DataContext = Container.ResolveNamed<MainWindowViewModel>(SecondaryExit)
                };
                secondaryWindow.Show();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}