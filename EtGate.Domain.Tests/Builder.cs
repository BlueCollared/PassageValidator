using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using Moq;
using System.Reactive.Linq;
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
            var mockQrReaderStatus = new Mock<IDeviceStatus<QrReaderStatus>>();
            var dummyQr = new Mock<IQrReaderController>();
            dummyQr.Setup(q => q.qrCodeInfoObservable).Returns(Observable.Empty<QrCodeInfo>());

            mockQrReaderStatus.Setup(m => m.statusObservable).Returns(r.subjQrStatus);

            r.qr = new QrReaderMgr(dummyQr.Object, mockQrReaderStatus.Object);

            var mockGateStatus = new Mock<IDeviceStatus<GateHwStatus>>();
            mockGateStatus.Setup(m => m.statusObservable).Returns(r.subjGateStatus);

            var dummyGate = new Mock<IGateController>();            
            r.gate = new GateMgr(dummyGate.Object, mockGateStatus.Object);            

            // Valiation
            var mockOfflineStatus = new Mock<IDeviceStatus<OfflineValidationSystemStatus>>();
            mockOfflineStatus.Setup(m => m.statusObservable).Returns(r.subjOfflineStatus);
            OfflineValidationSystem offline = new(mockOfflineStatus.Object);

            var mockOnlineStatus = new Mock<IDeviceStatus<OnlineValidationSystemStatus>>();
            mockOnlineStatus.Setup(m => m.statusObservable).Returns(r.subjOnlineStatus);            
            OnlineValidationSystem online = new(mockOnlineStatus.Object);

            r.validation = new ValidationMgr(offline, online);
            return r;
        }
    }
}
