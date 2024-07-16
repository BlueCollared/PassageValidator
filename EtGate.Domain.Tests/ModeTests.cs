using Domain;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using Moq;
using System.Reactive.Subjects;
using Xunit;

namespace EtGate.Domain.Tests
{
    public class ModeTests
    {
        [Fact]
        public void QrCodeNotWorking_ModeOOO()
        {
            //BehaviorSubject<int> x = new(5);
            //int r = x.Value;
            //x.OnNext(10);
            //int s = x.Value;
            // Arrange
            var qrReaderStatusSubject = new Subject<QrReaderStatus>();
            var mockQrReaderStatus = new Mock<IQrReaderStatus>();
            mockQrReaderStatus.Setup(m => m.qrReaderStatusObservable).Returns(qrReaderStatusSubject);

            var qrReaderMgr = new QrReaderMgr(null, mockQrReaderStatus.Object);
            var modeManager = new ModeManager(qrReaderMgr, null, null);

            // Act
            qrReaderStatusSubject.OnNext(new QrReaderStatus(bConnected:false, "", false)); // Simulate the QR reader status turning bad

            var z = modeManager.CurMode;
            // Assert
            Assert.Equal(Mode.OOO,
                //modeManager.GetMode()
                modeManager.CurMode
                );
                //modeManager.CurMode);
        }
    }
}