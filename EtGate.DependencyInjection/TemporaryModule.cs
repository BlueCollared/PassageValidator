using Autofac;
using Domain.Services.Modes;
using Equipment.Core.Message;
using Equipment.Core;
using EtGate.Domain.Services;
using EtGate.UI;
using EtGate.UI.ViewModels.Maintenance;
using GateApp;

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

    }
}
