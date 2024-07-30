using Autofac;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EtGate.UI.ViewModels;
using EtGate.UI.Views;
using GateApp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace EtGate.UI
{
    public partial class App : Avalonia.Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static Autofac.IContainer Container { get; private set; }

        public override void OnFrameworkInitializationCompleted()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);

            // Register your ViewModels and other services here
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<ModeService>().As<IModeService>();

            Container = builder.Build();

            var serviceProvider = new AutofacServiceProvider(Container);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = serviceProvider.GetService<MainWindowViewModel>()
                    //  DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            
        }
    }
}