using Autofac;
using EtGate.Domain.Services;
using GateApp;
using System.Reactive.Concurrency;

namespace EtGate.DependencyInjection;

public class TemporaryModule : Module
{
    private readonly bool bRealAlarmMgr;

    public TemporaryModule(OtherConf conf)
    {
        this.bRealAlarmMgr = conf.bRealAlarmMgr;
    }
    protected override void Load(ContainerBuilder builder)
    {
        if (bRealAlarmMgr)
            builder.RegisterType<EtGate.AlarmMgr.AlarmMgr>().SingleInstance().AsSelf()
                .AutoActivate();
        
        builder.RegisterType<MockContextRepository>().As<IContextRepository>().SingleInstance();
        builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
        builder.RegisterInstance(DefaultScheduler.Instance).As<IScheduler>();
        builder.RegisterType<SessionManager>()
            .As<ISessionManager>()
            .SingleInstance();        
    }
}

public class OtherConf
{
    public bool bRealAlarmMgr { get; set; }
}