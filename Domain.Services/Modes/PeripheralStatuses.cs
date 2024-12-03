using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;

namespace Domain.Services.Modes
{
    public class PeripheralStatuses
    {
        public DeviceStatusSubscriber<QrReaderStatus> qr { get; set; }
        public DeviceStatusSubscriber<OfflineValidationSystemStatus> offline { get; set; }
        public DeviceStatusSubscriber<OnlineValidationSystemStatus> online { get; set; }
        public DeviceStatusSubscriber<GateHwStatus> gate { get; set; }

        public static PeripheralStatuses ForTests()
        {
            PeripheralStatuses s = new();
            s.offline = new DeviceStatusSubscriberTest<OfflineValidationSystemStatus>();
            s.online = new DeviceStatusSubscriberTest<OnlineValidationSystemStatus>();
            s.gate = new DeviceStatusSubscriberTest<GateHwStatus>();
            s.qr = new DeviceStatusSubscriberTest<QrReaderStatus>();

            return s;
        }
    }
}