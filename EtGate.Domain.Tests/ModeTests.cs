using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.InService;
using Domain.Services.Modes;
using EtGate.Domain.ValidationSystem;
using Microsoft.Reactive.Testing;
using Moq;
using Shouldly;

namespace EtGate.Domain.Tests
{
    public class ModeTests
    {
        readonly TimeSpan timeToCompleteAppBoot = TimeSpan.FromSeconds(30);
        TestScheduler testScheduler = new TestScheduler();

        System s;
        ModeManager modeManager;
        Mock<ISubModeMgr> modeMgrMock;        

        public ModeTests()
        {
            s = new MockSytemBuilder().Build();

            var modeMgrFactoryMock = new Mock<ISubModeMgrFactory>();
            //modeMgrMock = new Mock<ISubModeMgr>();

            // Set up the mock to return the modeMgrMock when Create is called with any Mode value
            //modeMgrFactoryMock.Setup(f => f.Create(It.IsAny<Mode>())).Returns(modeMgrMock.Object);

            modeMgrFactoryMock
                .Setup(f => f.Create(It.IsAny<global::Domain.Mode>()))
                .Returns(() => {
                    modeMgrMock = new Mock<ISubModeMgr>();
                    return modeMgrMock.Object;
                });
            
            modeManager = new ModeManager(s.qr.StatusStream, s.validation.StatusStream, s.gate.StatusStream, modeMgrFactoryMock.Object, testScheduler);
        }

        [Fact]
        public void AppBootingPhase()
        {
            // Assert
            Assert.Equal(global::Domain.Mode.AppBooting, modeManager.CurMode);

            s.subjQrStatus.OnNext(QrReaderStatus.Disconnected);
            Assert.Equal(global::Domain.Mode.AppBooting, modeManager.CurMode);

            s.subjOfflineStatus.OnNext(OfflineValidationSystemStatus.Obsolete);
            Assert.Equal(global::Domain.Mode.AppBooting, modeManager.CurMode);

            s.subjOnlineStatus.OnNext(OnlineValidationSystemStatus.Disconnected);
            Assert.Equal(global::Domain.Mode.AppBooting, modeManager.CurMode);

            s.subjGateStatus.OnNext(GateHwStatus.Disconnected);
            Assert.NotEqual(global::Domain.Mode.AppBooting, modeManager.CurMode);
        }

        [Fact]
        public void AppBootingPhase_Timeout()
        {
            // Act
            testScheduler.AdvanceBy(TimeSpan.FromSeconds(ModeManager.DEFAULT_TimeToCompleteBoot_InSeconds).Ticks);

            Assert.NotEqual(global::Domain.Mode.AppBooting, modeManager.CurMode);
        }

        [Fact]
        public void QrCodeNotWorking_ModeOOO()
        {
            AllWorking_ModeInservice();
            // Act            
            s.subjQrStatus.OnNext(QrReaderStatus.Disconnected);

            // Assert
            Assert.Equal(global::Domain.Mode.OOO,
                modeManager.CurMode
                );

            // Act            
            s.subjQrStatus.OnNext(QrReaderStatus.AllGood);

            // Assert
            Assert.Equal(global::Domain.Mode.InService,
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
            // Assert
            Assert.Equal(global::Domain.Mode.AppBooting, modeManager.CurMode);

            s.subjQrStatus.OnNext(QrReaderStatus.AllGood);
            modeManager.CurMode.ShouldBe(global::Domain.Mode.AppBooting);

            s.subjOfflineStatus.OnNext(OfflineValidationSystemStatus.AllGood);
            modeManager.CurMode.ShouldBe(global::Domain.Mode.AppBooting);

            s.subjOnlineStatus.OnNext(OnlineValidationSystemStatus.Disconnected);

            s.subjGateStatus.OnNext(GateHwStatus.AllGood);
            modeManager.CurMode.ShouldBe(global::Domain.Mode.InService);
        }

        [Fact]
        public void OldModeMgrDisposed()
        {
            AllWorking_ModeInservice();
            
            var modeMgrMockBak = modeMgrMock;

            s.subjOfflineStatus.OnNext(OfflineValidationSystemStatus.Obsolete);
            Assert.Equal(global::Domain.Mode.OOO, modeManager.CurMode);
            //
            // 
            modeMgrMockBak.Verify(m => m.Dispose(), Times.Once);            
        }

        [Fact]
        public void InSer_OOO_InSer_NewModelMgrCreated()
        {
            AllWorking_ModeInservice();

            var subModeMgr = modeManager.curModeMgr;
            s.subjOfflineStatus.OnNext(OfflineValidationSystemStatus.Obsolete);
            modeManager.CurMode.ShouldBe(global::Domain.Mode.OOO);
            s.subjOfflineStatus.OnNext(OfflineValidationSystemStatus.AllGood);
            modeManager.CurMode.ShouldBe(global::Domain.Mode.InService);
            modeManager.curModeMgr.ShouldNotBeSameAs(subModeMgr);

        }
    }
}
