using Equipment.Core.Message;
using EtGate.Domain;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;
using System.Reactive.Linq;

namespace Domain.Services.Modes;

public class PeripheralStatuses
{
    public DeviceStatusSubscriber<QrReaderStatus> qr { get; set; }
    public DeviceStatusSubscriber<OfflineValidationSystemStatus> offline { get; set; }
    public DeviceStatusSubscriber<OnlineValidationSystemStatus> online { get; set; }
    public DeviceStatusSubscriber<GateHwStatus> gate { get; set; }
}

public class PeripheralStatuses_Test : PeripheralStatuses
{
    public DeviceStatusSubscriberTest<QrReaderStatus> qr_ { get; set; }
    public DeviceStatusSubscriberTest<OfflineValidationSystemStatus> offline_ { get; set; }
    public DeviceStatusSubscriberTest<OnlineValidationSystemStatus> online_ { get; set; }
    public DeviceStatusSubscriberTest<GateHwStatus> gate_ { get; set; }

    public static PeripheralStatuses_Test ForTests()
    {

        PeripheralStatuses_Test s = new();

        s.offline = s.offline_ = new DeviceStatusSubscriberTest<OfflineValidationSystemStatus>();
        s.online = s.online_ = new DeviceStatusSubscriberTest<OnlineValidationSystemStatus>();
        s.gate = s.gate_ = new DeviceStatusSubscriberTest<GateHwStatus>();
        s.qr = s.qr_ = new DeviceStatusSubscriberTest<QrReaderStatus>();

        return s;
    }

    public static PeripheralStatuses_Test AllGood()
    {
        PeripheralStatuses_Test s = ForTests();

        s.offline_.Publish(OfflineValidationSystemStatus.AllGood);
        s.online_.Publish(OnlineValidationSystemStatus.Disconnected);
        s.gate_.Publish(GateHwStatus.AllGood);
        s.qr_.Publish(QrReaderStatus.AllGood);

        return s;
    }

    public void SimulateStateChange()
    {
        qr.Messages.Publish(QrReaderStatus.AllGood);
        offline.Messages.Publish(OfflineValidationSystemStatus.AllGood);
        online.Messages.Publish(OnlineValidationSystemStatus.Disconnected);
        gate.Messages.Publish(GateHwStatus.AllGood);
    }

    public void SimulateStateChangeTo(Mode mode)
    {
    }
}