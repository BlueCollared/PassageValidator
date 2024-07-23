using Domain;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Qr;
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
            // Arrange
            var qrReaderStatusSubject = new Subject<QrReaderStatus>();
            var mockQrReaderStatus = new Mock<IQrReaderStatus>();
            mockQrReaderStatus.Setup(m => m.statusObservable).Returns(qrReaderStatusSubject);

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