using Domain.Peripherals.Qr;
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

        public readonly Subject<QrReaderStatus> subjQrStatus = new();
        public readonly Subject<ValidationSystemStatus> subjValidationStatus = new();
        public readonly Subject<OfflineValidationSystemStatus> subjOfflineStatus = new();
        public readonly Subject<OnlineValidationSystemStatus> subjOnlineStatus = new();
    }

    internal class MockSytemBuilder
    {
        public QrReaderStatus qrStatus = QrReaderStatus.AllGood;        

        public System Build()
        {
            System r = new();
            
            // Qr
            var mockQrReaderStatus = new Mock<IDeviceStatus<QrReaderStatus>>();
            var dummyQr = new Mock<IQrReader>();
            dummyQr.Setup(q => q.qrCodeInfoObservable).Returns(Observable.Empty<QrCodeInfo>());

            mockQrReaderStatus.Setup(m => m.statusObservable).Returns(r.subjQrStatus);

            r.qr = new QrReaderMgr(dummyQr.Object, mockQrReaderStatus.Object);

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
