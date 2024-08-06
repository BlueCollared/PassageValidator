using Domain;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using EtGate.Domain.ValidationSystem;
using Microsoft.Reactive.Testing;
using Shouldly;

namespace EtGate.Domain.Tests
{
    public class ModeTests
    {
        readonly TimeSpan timeToCompleteAppBoot = TimeSpan.FromSeconds(30);
        TestScheduler testScheduler = new TestScheduler();
        ModeManager CreateModeManager(System s)
        {
            return new ModeManager(s.qr, s.validation, null, testScheduler);
        }

        [Fact]
        public void AppBootingPhase()
        {
            // Arrange
            System s = new MockSytemBuilder().Build();
            ModeManager modeManager = CreateModeManager(s);

            // Assert
            Assert.Equal(Mode.AppBooting, modeManager.CurMode);

            s.subjQrStatus.OnNext(QrReaderStatus.Disconnected);
            Assert.Equal(Mode.AppBooting, modeManager.CurMode);

            s.subjOfflineStatus.OnNext(OfflineValidationSystemStatus.Obsolete);
            Assert.Equal(Mode.AppBooting, modeManager.CurMode);

            s.subjOnlineStatus.OnNext(OnlineValidationSystemStatus.Disconnected);
            Assert.NotEqual(Mode.AppBooting, modeManager.CurMode);
        }

        [Fact]
        public void AppBootingPhase_Timeout()
        {
            // Arrange
            System s = new MockSytemBuilder().Build();
            ModeManager modeManager = CreateModeManager(s);

            // Act
            testScheduler.AdvanceBy(TimeSpan.FromSeconds(ModeManager.DEFAULT_TimeToCompleteBoot_InSeconds).Ticks);

            Assert.NotEqual(Mode.AppBooting, modeManager.CurMode);
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
            // Arrange
            System s = new MockSytemBuilder().Build();
            ModeManager modeManager = CreateModeManager(s);

            // Assert
            Assert.Equal(Mode.AppBooting, modeManager.CurMode);

            s.subjQrStatus.OnNext(QrReaderStatus.AllGood);
            modeManager.CurMode.ShouldBe(Mode.AppBooting);

            s.subjOfflineStatus.OnNext(OfflineValidationSystemStatus.AllGood);
            Assert.Equal(Mode.AppBooting, modeManager.CurMode);

            s.subjOnlineStatus.OnNext(OnlineValidationSystemStatus.Disconnected);
            modeManager.CurMode.ShouldBe(Mode.InService);
        }
    }
}