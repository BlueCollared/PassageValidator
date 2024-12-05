using Autofac;
using Domain.Services.Modes;
using Equipment.Core.Message;
using Equipment.Core;
using EtGate.Domain.Services;
using EtGate.UI;
using EtGate.UI.ViewModels.Maintenance;
using GateApp;
using EtGate.UI.ViewModels;
using LanguageExt.ClassInstances;

namespace EtGate.DependencyInjection;

public class TemporaryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockContextRepository>().As<IContextRepository>().SingleInstance();
        builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();

        // This way we register all viewmodels in one go
        var viewModelAssembly = typeof(AgentLoginViewModel).Assembly;
        builder.RegisterAssemblyTypes(viewModelAssembly);

        builder.RegisterType<PeripheralStatuses>()
            .AsSelf()
            .SingleInstance()
            .PropertiesAutowired();

        builder.RegisterGeneric(typeof(EventBus<>))
            .As(typeof(IMessagePublisher<>))
            .As(typeof(IMessageSubscriber<>))
            .SingleInstance();

        builder.RegisterGeneric(typeof(DeviceStatusBus<>))
            .As(typeof(IDeviceStatusPublisher<>))
            .As(typeof(DeviceStatusSubscriber<>))
            .SingleInstance();

        const string bPrimary = "bPrimary";
        const string bEntry = "bEntry";

                const string PrimaryEntry = nameof(PrimaryEntry);
        const string PrimaryExit = nameof(PrimaryExit);
        const string SecondaryExit = nameof(SecondaryExit);
        const string SecondaryEntry = nameof(SecondaryEntry);


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

        builder.Register<Func<Type, MaintainenaceViewModelBase>>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return viewModelType => (MaintainenaceViewModelBase)componentContext.Resolve(viewModelType);
        });

        AutoFacConfig.RegisterViewModels_ExceptRootVM(builder);

    }
}
