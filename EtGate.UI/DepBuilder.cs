using Autofac;
using Domain.Services.Modes;
using Equipment.Core.Message;
using Equipment.Core;
using EtGate.UI.ViewModels.Maintenance;
using EtGate.UI.ViewModels;
using System.Reactive.Concurrency;
using System;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Modes;
using EtGate.Domain.Services;
using GateApp;

namespace EtGate.UI;
public static class DepBuilder
{
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
           );

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