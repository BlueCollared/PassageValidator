using Domain;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using EtGate.Domain.Services.Qr;
using Moq;
using System.Reactive.Subjects;

namespace EtGate.Domain.Tests
{
    public class ModeTests
    {
        ModeManager CreateModeManager(System s)
        {
            return new ModeManager(s.qr, s.validation, null);
        }

        [Fact]
        public void QrCodeNotWorking_ModeOOO()
        {
            // Arrange
            System s = new MockSytemBuilder().Build();            
            ModeManager modeManager = CreateModeManager(s);

            // Act            
            s.subjQrStatus.OnNext(QrReaderStatus.Disconnected);
            
            // Assert
            Assert.Equal(Mode.OOO,                
                modeManager.CurMode
                );

            // Act            
            s.subjQrStatus.OnNext(QrReaderStatus.AllGood);

            // Assert
            Assert.Equal(Mode.InService,
                modeManager.CurMode
                );
        }

        [Fact]
        public void ValidationMgrNotWorking_ModeOOO()
        {

        }

        [Fact]
        public void GateHWNotWorking_ModeOOO()
        {

        }

        [Fact]
        public void AllWorking_ModeInservice()
        {

        }
    }
}