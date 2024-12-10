using Autofac;
using Domain.Services.Modes;
using EtGate.Domain.Services;
using GateApp;
using System.Reactive.Concurrency;

namespace EtGate.DependencyInjection;

public class TemporaryModule : Module
{
    private readonly OtherConf conf;

    public TemporaryModule(OtherConf conf)
    {
        this.conf = conf;
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (conf.bRealAlarmMgr)
            builder.RegisterType<EtGate.AlarmMgr.AlarmMgr>().SingleInstance().AsSelf()
                .AutoActivate();
        
        builder.RegisterType<MockContextRepository>().As<IContextRepository>().SingleInstance();
        builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
        builder.RegisterInstance(DefaultScheduler.Instance).As<IScheduler>();
        builder.RegisterType<SessionManager>()
            .As<ISessionManager>()
            .SingleInstance();

        if (!conf.bForceFulAllGood)
            builder.RegisterType<PeripheralStatuses>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired()
                ;
        else
        {
            // TODO: replace it with individual components
            PeripheralStatuses_Test p = PeripheralStatuses_Test.AllGood();

            var x = p.gate.curStatus;
            p.gate.Messages.Subscribe(x => { });
            builder.RegisterInstance(p)
                .SingleInstance()
                //.PropertiesAutowired()
                .As<PeripheralStatuses>();
        }
    }
}

public class OtherConf
{
    public bool bRealAlarmMgr { get; set; } = true;
    public bool bForceFulAllGood { get; set; } = false;
}