using Autofac;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EtGate.UI.ViewModels;
using EtGate.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reactive.Concurrency;

namespace EtGate.UI;
public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static Autofac.IContainer? Container { get; private set; }

    public override void OnFrameworkInitializationCompleted()
    {
        var serviceCollection = new ServiceCollection();
        //ConfigureServices(serviceCollection);

        var builder = new ContainerBuilder();
        builder.Populate(serviceCollection);
        builder.RegisterInstance(DefaultScheduler.Instance).As<IScheduler>();

        const string PrimaryEntry = nameof(PrimaryEntry);
        const string PrimaryExit = nameof(PrimaryExit);
        const string SecondaryExit = nameof(SecondaryExit);
        const string SecondaryEntry = nameof(SecondaryEntry);

        const string bPrimary = "bPrimary";
        const string bEntry = "bEntry";

        builder.RegisterType<ModeViewModelFactory>().As<IModeViewModelFactory>().SingleInstance();

        builder.RegisterType<MainWindowViewModel>()
            .WithParameter(new NamedParameter(bPrimary, true))
            .WithParameter(new NamedParameter(bEntry, true))
            .Named<MainWindowViewModel>(PrimaryEntry);

        builder.RegisterType<MainWindowViewModel>()
            .WithParameter(new NamedParameter(bPrimary, false))
            .WithParameter(new NamedParameter(bEntry, true))
            .Named<MainWindowViewModel>(SecondaryEntry);

        builder.RegisterType<MainWindowViewModel>()
            .WithParameter(new NamedParameter(bPrimary, true))
            .WithParameter(new NamedParameter(bEntry, false))
            .Named<MainWindowViewModel>(PrimaryExit);

        builder.RegisterType<MainWindowViewModel>()
            .WithParameter(new NamedParameter(bPrimary, false))
            .WithParameter(new NamedParameter(bEntry, false))
            .Named<MainWindowViewModel>(SecondaryExit);

        //builder.RegisterType<ModeServiceLocalAgent>().As<IModeCommandService>().SingleInstance();
        
        AutoFacConfig.RegisterViewModels_ExceptRootVM(builder);

        builder.Register<Func<Type, MaintainenaceViewModelBase>>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return viewModelType => (MaintainenaceViewModelBase)componentContext.Resolve(viewModelType);
        });
        
        builder.RegisterType<MaintenanceNavigationService>()
           .As<INavigationService>()
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(Func<Type, MaintainenaceViewModelBase>),
               (pi, ctx) => ctx.Resolve<Func<Type, MaintainenaceViewModelBase>>()
           );
        
        builder.RegisterType<NavigationEventManager>().As<INavigationEventManager>().SingleInstance();
        builder.RegisterType<MaintenanceViewFactory>().As<IViewFactory>().SingleInstance();


        Container = builder.Build();

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