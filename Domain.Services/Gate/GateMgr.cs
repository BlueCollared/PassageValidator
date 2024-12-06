using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Services.Gate.Functions;
using System.Reactive.Linq;

namespace EtGate.Domain.Services.Gate;

public class GateMgr
{
    public class Config
    {
        public ClockSynchronizer.Config ClockSynchronizerConfig { get; set; } = new();
    }

    public IDeviceDate deviceDate { get; private set; }
    private readonly DeviceStatusSubscriber<GateHwStatus> statusStream;

    public IObservable<GateHwStatus> StatusStream => statusStream.Messages;

    GateConnectedSession session;

    Func<ClockSynchronizer> ClockSynchronizerFactory;
    Config config;

    public GateMgr(IDeviceDate deviceDate,
        DeviceStatusSubscriber<GateHwStatus> statusStream,
        Config config
        )
    {
        this.deviceDate = deviceDate ?? throw new ArgumentNullException(nameof(deviceDate));
        this.statusStream = statusStream ?? throw new ArgumentNullException(nameof(statusStream));
        this.config = config ?? throw new ArgumentNullException(nameof(config));

        ClockSynchronizerFactory = () =>
        {
            return new ClockSynchronizer(deviceDate
                , () => DateTimeOffset.Now
                , (ClockSynchronizer.DateChanged d) => { }
                , this.config.ClockSynchronizerConfig);
        };

        statusStream.Messages
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
