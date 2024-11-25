using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;
using Microsoft.Reactive.Testing;
using Moq;

namespace EtGate.Domain.Tests
{
    public class InserviceTests
    {
        DeviceStatusSubscriberTest<QrReaderStatus> qr = new();
        DeviceStatusSubscriberTest<OfflineValidationSystemStatus> offline = new();
        DeviceStatusSubscriberTest<OnlineValidationSystemStatus> online = new();
        DeviceStatusSubscriberTest<GateHwStatus> gate = new();
        TestScheduler testScheduler = new TestScheduler();

        ModeManager modeManager;
        Mock<ISubModeMgr> modeMgrSub;

        public InserviceTests()
        {
            var modeMgrFactoryMock = new Mock<ISubModeMgrFactory>();
            //modeMgrMock = new Mock<ISubModeMgr>();

            // Set up the mock to return the modeMgrMock when Create is called with any Mode value
            //modeMgrFactoryMock.Setup(f => f.Create(It.IsAny<Mode>())).Returns(modeMgrMock.Object);

            modeMgrFactoryMock
                .Setup(f => f.Create(It.IsAny<global::EtGate.Domain.Mode>()))
                .Returns(() => {
                    modeMgrSub = new Mock<ISubModeMgr>();
                    return modeMgrSub.Object;
                });

            modeManager = new ModeManager(qr, offline, online, gate, (new Mock<IDeviceStatusPublisher<Mode>>()).Object, modeMgrFactoryMock.Object, testScheduler);
        }

        [Fact]
        public void WhenSystemGoesInService_QrPutInDetectingMode()
        {

        }

        [Fact]
        public void ValidationSystemGivingThumbsupToAQr_MakesFlapOpen()
        {

        }

        [Fact]
        public void QrDetectionStopsAfterV()
        {

        }
    }
}
