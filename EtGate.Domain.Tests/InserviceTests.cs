using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
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
        DeviceStatusSubscriberTest<ActiveFunctionalities> activeFns = new();
        TestScheduler testScheduler = new TestScheduler();

        IGateInServiceController gateInServiceController;

        ModeEvaluator modeManager;
        Mock<ISubModeMgr> modeMgrSub;
        private readonly ValidationMgr validationMgr;
        private readonly IQrReaderMgr qrMgr;
        IValidate onlineValidMgr, offlineValidMgr;
        public InserviceTests()
        {
            onlineValidMgr = new OnlineValidationSystem(new Mock<IDeviceStatusPublisher<OnlineValidationSystemStatus>>().Object);
            offlineValidMgr = new OfflineValidationSystem(new Mock<IDeviceStatusPublisher<OfflineValidationSystemStatus>>().Object);
            gateInServiceController = new Mock<IGateInServiceController>().Object;
            validationMgr = new ValidationMgr(onlineValidMgr, online, offlineValidMgr, offline);
            qrMgr = new Mock<IQrReaderMgr>().Object;
            var modeMgrFactoryMock = new InServiceMgr(validationMgr, gateInServiceController, qrMgr, activeFns);
            //modeMgrMock = new Mock<ISubModeMgr>();

            // Set up the mock to return the modeMgrMock when Create is called with any Mode value
            //modeMgrFactoryMock.Setup(f => f.Create(It.IsAny<Mode>())).Returns(modeMgrMock.Object);

            //modeMgrFactoryMock
            //    .Setup(f => f.Create(It.IsAny<global::EtGate.Domain.Mode>()))
            //    .Returns(() => {
            //        modeMgrSub = new Mock<ISubModeMgr>();
            //        return modeMgrSub.Object;
            //    });
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
