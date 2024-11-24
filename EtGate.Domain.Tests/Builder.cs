using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using Moq;
using System.Reactive.Subjects;

namespace EtGate.Domain.Tests
{
    internal class System
    {        
        public ValidationMgr validation;
        public QrReaderMgr qr;
        public GateMgr gate;

        public readonly Subject<QrReaderStatus> subjQrStatus = new();
        public readonly Subject<ValidationSystemStatus> subjValidationStatus = new();
        public readonly Subject<OfflineValidationSystemStatus> subjOfflineStatus = new();
        public readonly Subject<OnlineValidationSystemStatus> subjOnlineStatus = new();
        public readonly Subject<GateHwStatus> subjGateStatus = new();
    }

    internal class MockSytemBuilder
    {
        public QrReaderStatus qrStatus = QrReaderStatus.AllGood;        

        public System Build()
        {
            System r = new();
            
            // Qr
            var mockQrReaderStatus = new Mock<DeviceStatusSubscriber<QrReaderStatus>>();
            var dummyQr = new Mock<IQrReaderController>();
            //dummyQr.Setup(q => q.qrCodeInfoObservable).Returns(Observable.Empty<QrCodeInfo>());

            mockQrReaderStatus.Setup(m => m.Messages).Returns(r.subjQrStatus);

            r.qr = new QrReaderMgr(dummyQr.Object, mockQrReaderStatus.Object);

            var mockGateStatus = new Mock<IMessageSubscriber<GateHwStatus>>();
            mockGateStatus.Setup(m => m.Messages).Returns(r.subjGateStatus);

            var dummyGate = new Mock<IDeviceDate>();            
            r.gate = new GateMgr(dummyGate.Object, new DeviceStatusBus<GateHwStatus>(), new GateMgr.Config());            

            // Valiation
            var mockOfflineStatus = new Mock<DeviceStatusBus<OfflineValidationSystemStatus>>();
            mockOfflineStatus.Setup(m => m.Messages).Returns(r.subjOfflineStatus);
            OfflineValidationSystem offline = new(mockOfflineStatus.Object);

            var mockOnlineStatus = new Mock<DeviceStatusBus<OnlineValidationSystemStatus>>();
            mockOnlineStatus.Setup(m => m.Messages).Returns(r.subjOnlineStatus);            
            OnlineValidationSystem online = new(mockOnlineStatus.Object);

            r.validation = new ValidationMgr(online, mockOnlineStatus.Object, offline, mockOfflineStatus.Object);
            return r;
        }
    }
}
