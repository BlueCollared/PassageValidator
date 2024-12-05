using Autofac;
using EtGate.DependencyInjection.Mocks;
using EtGate.Devices.IER;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Gate.Functions;
using EtGate.IER;
using Horizon.XmlRpc.Client;

namespace EtGate.DependencyInjection;

public enum GateDepNature
{
    Real,
    AlwaysGood,
    AlwaysBad,
    SimulatorDriven
}

public class GateModule : Module
{
    private readonly GateDepNature dep;

    public GateModule(GateDepNature dep)
    {
        this.dep = dep;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // TODO: correct this switch staement

        switch (dep)
        {
            case GateDepNature.Real:
                {
                    string url = "https://www.google.com/"; // TODO: load from configuration
                    var xmlRpc = XmlRpcProxyGen.Create<IIERXmlRpcInterface>();
                    xmlRpc.Url = url;
                    var ierRpc = new IerActive(new IERXmlRpc(xmlRpc, null));
                    var ier = new IerController(ierRpc, ierRpc);

                    builder.RegisterInstance(ier)
                        .As<IGateController>()
                        //.As<IDeviceStatusPublisher<GateHwStatus>>()
                        .As<IDeviceDate>() // TODO: this is WRONG to assume that only IerController implements IDeveiceDate
                        .SingleInstance();

                    builder.RegisterInstance(new GateMgr.Config { ClockSynchronizerConfig = new ClockSynchronizer.Config { interval = TimeSpan.FromMinutes(5) } })
                        .As<GateMgr.Config>()
                        .SingleInstance();

                    builder.RegisterType<GateMgr>().AsSelf().SingleInstance();
                    builder.RegisterType<MockPassageManager>().As<IGateInServiceController>();
                    break;
                }
            default:
                throw new NotImplementedException();
        }
    }
}