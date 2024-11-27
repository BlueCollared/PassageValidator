using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;
using Microsoft.Reactive.Testing;
using Moq;
using Shouldly;

namespace EtGate.Domain.Tests
{
    public class ModeManagerTests
    {
        readonly TimeSpan timeToCompleteAppBoot = TimeSpan.FromSeconds(30);
        TestScheduler testScheduler = new TestScheduler();
        
        ModeManager modeManager;
        Mock<ISubModeMgr> modeMgrMock;
        //Mock<IMessageSubscriber<QrReaderStatus>> mockQr = new();
        DeviceStatusSubscriberTest<QrReaderStatus> qr = new();
        DeviceStatusSubscriberTest<OfflineValidationSystemStatus> offline = new();
        DeviceStatusSubscriberTest<OnlineValidationSystemStatus> online = new();
        DeviceStatusSubscriberTest<GateHwStatus> gate = new();

        public ModeManagerTests()
        {
            var modeMgrFactoryMock = new Mock<ISubModeMgrFactory>();
            //modeMgrMock = new Mock<ISubModeMgr>();

            // Set up the mock to return the modeMgrMock when Create is called with any Mode value
            //modeMgrFactoryMock.Setup(f => f.Create(It.IsAny<Mode>())).Returns(modeMgrMock.Object);

            modeMgrFactoryMock
                .Setup(f => f.Create(It.IsAny<global::EtGate.Domain.Mode>()))
                .Returns(() => {
                    modeMgrMock = new Mock<ISubModeMgr>();
                    return modeMgrMock.Object;
                });

            modeManager = new ModeManager(qr, offline, online, gate, modePub:null, activeFuncsPub:null, modeMgrFactoryMock.Object, testScheduler);
        }

        [Fact]
        public void AppBootingPhase()
        {
            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, modeManager.CurMode);

            qr.Publish(QrReaderStatus.Disconnected);
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, modeManager.CurMode);

            offline.Publish(OfflineValidationSystemStatus.Obsolete);
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, modeManager.CurMode);

            online.Publish(OnlineValidationSystemStatus.Disconnected);
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, modeManager.CurMode);

            gate.Publish(GateHwStatus.Disconnected);
            Assert.NotEqual(global::EtGate.Domain.Mode.AppBooting, modeManager.CurMode);
        }

        [Fact]
        public void AppBootingPhase_Timeout()
        {
            // Act
            testScheduler.AdvanceBy(TimeSpan.FromSeconds(ModeManager.DEFAULT_TimeToCompleteBoot_InSeconds).Ticks);

            Assert.NotEqual(global::EtGate.Domain.Mode.AppBooting, modeManager.CurMode);
        }

        [Fact]
        public void QrCodeNotWorking_ModeOOO()
        {
            AllWorking_ModeInservice();
            // Act            
            qr.Publish(QrReaderStatus.Disconnected);

            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.OOO,
                modeManager.CurMode
                );

            // Act            
            qr.Publish(QrReaderStatus.AllGood);

            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.InService,
                modeManager.CurMode
                );
        }

        [Fact]
        public void ValidationMgrNotWorking_ModeOOO()
        {
            AllWorking_ModeInservice();

            offline.Publish(OfflineValidationSystemStatus.Obsolete);
            online.Publish(OnlineValidationSystemStatus.Disconnected);
            
            Assert.Equal(global::EtGate.Domain.Mode.OOO,
                modeManager.CurMode
                );

            offline.Publish(OfflineValidationSystemStatus.AllGood);
            Assert.Equal(global::EtGate.Domain.Mode.InService,
                modeManager.CurMode
                );
        }

        [Fact]
        public void GateHWNotWorking_ModeOOO()
        {
            AllWorking_ModeInservice();
            // Act            
            gate.Publish(GateHwStatus.Disconnected);

            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.OOO,
                modeManager.CurMode
                );

            // Act            
            gate.Publish(GateHwStatus.AllGood);

            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.InService,
                modeManager.CurMode
                );
        }

        [Fact]
        public void AllWorking_ModeInservice()
        {
            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, modeManager.CurMode);

            qr.Publish(QrReaderStatus.AllGood);
            modeManager.CurMode.ShouldBe(global::EtGate.Domain.Mode.AppBooting);

            offline.Publish(OfflineValidationSystemStatus.AllGood);
            modeManager.CurMode.ShouldBe(global::EtGate.Domain.Mode.AppBooting);

            online.Publish(OnlineValidationSystemStatus.Disconnected);

            gate.Publish(GateHwStatus.AllGood);
            modeManager.CurMode.ShouldBe(global::EtGate.Domain.Mode.InService);
        }

        [Fact]
        public void OldModeMgrDisposed()
        {
            AllWorking_ModeInservice();
            
            var modeMgrMockBak = modeMgrMock;

            offline.Publish(OfflineValidationSystemStatus.Obsolete);
            Assert.Equal(global::EtGate.Domain.Mode.OOO, modeManager.CurMode);
            //
            // 
            modeMgrMockBak.Verify(m => m.Dispose(), Times.Once);            
        }

        [Fact]
        public void InSer_OOO_InSer_NewModelMgrCreated()
        {
            AllWorking_ModeInservice();

            var subModeMgr = modeManager.curModeMgr;
            offline.Publish(OfflineValidationSystemStatus.Obsolete);
            modeManager.CurMode.ShouldBe(global::EtGate.Domain.Mode.OOO);
            offline.Publish(OfflineValidationSystemStatus.AllGood);
            modeManager.CurMode.ShouldBe(global::EtGate.Domain.Mode.InService);
            modeManager.curModeMgr.ShouldNotBeSameAs(subModeMgr);

        }
    }
}
