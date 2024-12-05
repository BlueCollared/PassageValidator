using Autofac;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Markup.Xaml;
using EtGate.UI.ViewModels;
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

        //builder.RegisterType<ModeServiceLocalAgent>().As<IModeCommandService>().SingleInstance();       

        
        builder.RegisterType<MaintenanceNavigationService>()
           .As<INavigationService>()
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(Func<Type, MaintainenaceViewModelBase>),
               (pi, ctx) => ctx.Resolve<Func<Type, MaintainenaceViewModelBase>>()
           );
        
        builder.RegisterType<NavigationEventManager>().As<INavigationEventManager>().SingleInstance();
        builder.RegisterType<MaintenanceViewFactory>().As<IViewFactory>().SingleInstance();
    }
}