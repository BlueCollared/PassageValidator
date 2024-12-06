using Autofac;
using Domain.Services.Modes;
using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Modes;
using EtGate.UI.ViewModels;
using EtGate.UI.ViewModels.Maintenance;
using EtGate.UI.Views;
using System;
using System.Reactive.Concurrency;

namespace EtGate.UI;
public static class DepBuilder
{
    // a back-door entry to the DI container, so that it can be used as service locator; but reseve it only only for exceptional scenarios
    public static IContainer Container { get; set; }

    public static void Do(ContainerBuilder builder)
    {        
        const string PrimaryEntry = nameof(PrimaryEntry);
        const string PrimaryExit = nameof(PrimaryExit);
        const string SecondaryExit = nameof(SecondaryExit);
        const string SecondaryEntry = nameof(SecondaryEntry);

        const string bPrimary = "bPrimary";
        const string bEntry = "bEntry";

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
        
        AutoFacConfig.RegisterViewModels_ExceptRootVM(builder);

        builder.Register<Func<Type, MaintainenaceViewModelBase>>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return viewModelType => (MaintainenaceViewModelBase)componentContext.Resolve(viewModelType);
        });

        var viewModelAssembly = typeof(AgentLoginViewModel).Assembly;
        builder.RegisterAssemblyTypes(viewModelAssembly);

        builder.RegisterType<MaintenanceNavigationService>()
           .As<INavigationService>()
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(Func<Type, MaintainenaceViewModelBase>),
               (pi, ctx) => ctx.Resolve<Func<Type, MaintainenaceViewModelBase>>()
           ).SingleInstance();

        builder.RegisterType<ModeViewModelFactory>().As<IModeViewModelFactory>().SingleInstance();

        builder.RegisterType<SubModeMgrFactory>().As<ISubModeMgrFactory>().SingleInstance();

        builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
        builder.RegisterType<NavigationEventManager>().As<INavigationEventManager>().SingleInstance();
        builder.RegisterType<MaintenanceViewFactory>().As<IViewFactory>().SingleInstance();

        builder.RegisterGeneric(typeof(EventBus<>))
       .As(typeof(IMessagePublisher<>))
       .As(typeof(IMessageSubscriber<>))
       .SingleInstance();

        builder.RegisterGeneric(typeof(DeviceStatusBus<>))
       .As(typeof(IDeviceStatusPublisher<>))
       .As(typeof(DeviceStatusSubscriber<>))
       .SingleInstance();

        builder.RegisterType<ModeFacade>()
            .WithParameter("scheduler", new EventLoopScheduler())
            .WithParameter("timeToCompleteAppBoot_InSeconds", 30)
            .As<IModeManager>()
            .SingleInstance()
            .AsSelf();        

        builder.RegisterType<PeripheralStatuses>()
            .AsSelf()
            .SingleInstance()
            .PropertiesAutowired();
    }
}