using Autofac;
using EtGate.Domain.Services;
using GateApp;
using System.Reactive.Concurrency;

namespace EtGate.DependencyInjection;

public class TemporaryModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MockContextRepository>().As<IContextRepository>().SingleInstance();
        builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
        builder.RegisterInstance(DefaultScheduler.Instance).As<IScheduler>();
    }
}