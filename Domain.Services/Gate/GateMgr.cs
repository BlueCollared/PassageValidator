using Domain.Peripherals.Passage;
using EtGate.Domain.Services.Gate.Functions;
using System.Reactive.Linq;

namespace EtGate.Domain.Services.Gate;

public class GateMgr
{
    public class Config
    {
        public ClockSynchronizer.Config ClockSynchronizerConfig { get; set; } = new();
    }

    public IDeviceDate deviceDate { get; private set;}
    private readonly IDeviceStatus<GateHwStatus> statusMgr;

    public IObservable<GateHwStatus> StatusStream => statusMgr.statusObservable;

    GateConnectedSession session;

    Func<ClockSynchronizer> ClockSynchronizerFactory;
    Config config;

    public GateMgr(IDeviceDate deviceDate,
        IDeviceStatus<GateHwStatus> statusMgr,
        Config config
        )
    {
        this.deviceDate = deviceDate ?? throw new ArgumentNullException(nameof(deviceDate));
        this.statusMgr = statusMgr ?? throw new ArgumentNullException(nameof(statusMgr));
        this.config = config ?? throw new ArgumentNullException(nameof(config));

        ClockSynchronizerFactory = () => {
            return new ClockSynchronizer(deviceDate
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
                    session = new GateConnectedSession(ClockSynchronizerFactory);                    
            });
    }
}
