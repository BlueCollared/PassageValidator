using Domain.Peripherals.Passage;
using System.Reactive.Linq;

namespace EtGate.Domain.Services.Gate;

public class GateMgr
{
    public class Config
    {
        public ClockSynchronizer.Config ClockSynchronizerConfig { get; set; } = new();
    }

    public IGateController gateController { get; private set;}
    private readonly IDeviceStatus<GateHwStatus> statusMgr;

    public IObservable<GateHwStatus> StatusStream => statusMgr.statusObservable;

    GateConnectedSession session;

    Func<ClockSynchronizer> ClockSynchronizerFactory;
    Config config;

    public GateMgr(IGateController gateController,
        IDeviceStatus<GateHwStatus> statusMgr,
        Config config
        )
    {
        this.gateController = gateController ?? throw new ArgumentNullException(nameof(gateController));
        this.statusMgr = statusMgr ?? throw new ArgumentNullException(nameof(statusMgr));
        this.config = config ?? throw new ArgumentNullException(nameof(config));

        ClockSynchronizerFactory = () => {
            return new ClockSynchronizer(gateController
                , ()=>DateTimeOffset.Now
                , (ClockSynchronizer.DateChanged d)=>{}
                , this.config.ClockSynchronizerConfig);
        };

            statusMgr.statusObservable
            .Select(x => x.bConnected)
            .DistinctUntilChanged()
            .Subscribe(bConnected =>
            {
                if (!bConnected)
                    session?.Dispose();
                else
                    session = new GateConnectedSession(this.gateController, ClockSynchronizerFactory);                    
            });
    }
}
