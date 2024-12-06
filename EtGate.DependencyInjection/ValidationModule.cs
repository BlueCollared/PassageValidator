using Autofac;
using DummyQrReaderDeviceController;
using Equipment.Core.Message;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;

namespace EtGate.DependencyInjection;

public enum ValidationDepNature
{
    Real,
    AlwaysGood,
    AlwaysBad,
    SimulatorDriven
}

public class ValidationModule : Module
{
    private readonly ValidationDepNature dep;

    const string offlineValidatorMnemonic = "OffLine";
    const string onlineValidatorMnemonic = "OnLine";

    public ValidationModule(ValidationDepNature dep)
    {
        this.dep = dep;
    }

    protected override void Load(ContainerBuilder builder)
    {
        switch (dep)
        {
            case ValidationDepNature.Real:
                {
                    builder.RegisterType<DummyOfflineValidation>()
    .SingleInstance()
    .Named<IValidate>(offlineValidatorMnemonic);

                    builder.RegisterType<DummyOnlineValidation>()
                       .SingleInstance()
                       .Named<IValidate>(onlineValidatorMnemonic);

                    builder.RegisterType<OfflineValidationSystem>().AsSelf()
                        .SingleInstance();
                    builder.RegisterType<OnlineValidationSystem>().AsSelf()
                        .SingleInstance();
                    //builder.RegisterType<ValidationMgr>().AsSelf().SingleInstance();
                    builder.RegisterType<ValidationMgr>()
                           .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(IValidate) && pi.Name == "online",
                               (pi, ctx) => ctx.ResolveNamed<IValidate>(onlineValidatorMnemonic))
                           .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(DeviceStatusSubscriber<OnlineValidationSystemStatus>) && pi.Name == "onlineDeviceStatus",
                               (pi, ctx) => ctx.Resolve<DeviceStatusSubscriber<OnlineValidationSystemStatus>>())
                           .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(IValidate) && pi.Name == "offline",
                               (pi, ctx) => ctx.ResolveNamed<IValidate>(offlineValidatorMnemonic))
                           .WithParameter(
                               (pi, ctx) => pi.ParameterType == typeof(DeviceStatusSubscriber<OfflineValidationSystemStatus>) && pi.Name == "offlineDeviceStatus",
                               (pi, ctx) => ctx.Resolve<DeviceStatusSubscriber<OfflineValidationSystemStatus>>())
                        .SingleInstance()
                        .AsSelf();

                    break;

                }
        }
    }
}