using Autofac;
using Domain.Services.Modes;
using EtGate.Domain.Services.Modes;
using System.Reactive.Concurrency;

namespace EtGate.DependencyInjection;

public class ModeModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ModeFacade>()
            .WithParameter("scheduler", new EventLoopScheduler())
            .WithParameter("timeToCompleteAppBoot_InSeconds", 30)
            .As<IModeManager>()
            .SingleInstance()
            .AsSelf();

        builder.RegisterType<SubModeMgrFactory>().As<ISubModeMgrFactory>().SingleInstance();
    }
}
