using Autofac;
using EtGate.Domain.Services.Qr;

namespace EtGate.DependencyInjection;

public enum QrDepNature
{
    Real,
    AlwaysGood,
    AlwaysBad,
    SimulatorDriven
}

public class QrModule : Module
{
    private readonly QrDepNature dep;

    public QrModule(QrDepNature dep)
    {
        this.dep = dep;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // TODO: correct this switch staement
        switch (dep)
        {
            case QrDepNature.Real:
                builder.RegisterType<DummyQrReaderDeviceController.DummyQrReaderDeviceController>()
                    .As<IQrReaderController>()
                    .SingleInstance();

                builder.RegisterType<QrReaderMgr>()
                   .WithParameter(
                       (pi, ctx) => pi.ParameterType == typeof(IQrReaderController),
                       (pi, ctx) => ctx.Resolve<IQrReaderController>())
                   .As<IQrReaderMgr>()
                   .SingleInstance();
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
