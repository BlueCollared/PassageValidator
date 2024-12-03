using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;
using Microsoft.Reactive.Testing;
using Moq;
using Shouldly;

namespace EtGate.Domain.Tests
{
    public class ModeEvaluatorTests
    {
        readonly TimeSpan timeToCompleteAppBoot = TimeSpan.FromSeconds(30);
        TestScheduler testScheduler = new TestScheduler();
        
        ModeEvaluator modeManager;
        Mock<ISubModeMgr> modeMgrMock;

        PeripheralStatuses ps = PeripheralStatuses.ForTests();
        DeviceStatusSubscriberTest<QrReaderStatus> qr;
        DeviceStatusSubscriberTest<OfflineValidationSystemStatus> offline;
        DeviceStatusSubscriberTest<OnlineValidationSystemStatus> online;
        DeviceStatusSubscriberTest<GateHwStatus> gate;

        DeviceStatusBus<(Mode, bool)> modePub = new();
        Mode CurMode;

        public ModeEvaluatorTests()
        {
            qr = (DeviceStatusSubscriberTest<QrReaderStatus>)ps.qr;
            offline = (DeviceStatusSubscriberTest<OfflineValidationSystemStatus>)ps.offline;
            online = (DeviceStatusSubscriberTest<OnlineValidationSystemStatus>)ps.online;
            gate = (DeviceStatusSubscriberTest<GateHwStatus>)ps.gate;

            modePub.Messages.Subscribe(x => {
                CurMode = x.Item1;
            });
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

            modeManager = new ModeEvaluator(ps, modePub, null, testScheduler);
        }

        [Fact]
        public void AppBootingPhase()
        {
            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, CurMode);

            qr.Publish(QrReaderStatus.Disconnected);
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, CurMode);

            offline.Publish(OfflineValidationSystemStatus.Obsolete);
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, CurMode);

            online.Publish(OnlineValidationSystemStatus.Disconnected);
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, CurMode);

            gate.Publish(GateHwStatus.Disconnected);
            Assert.NotEqual(global::EtGate.Domain.Mode.AppBooting, CurMode);
        }

        [Fact]
        public void AppBootingPhase_Timeout()
        {
            // Act
            testScheduler.AdvanceBy(TimeSpan.FromSeconds(ModeEvaluator.DEFAULT_TimeToCompleteBoot_InSeconds).Ticks);

            Assert.NotEqual(global::EtGate.Domain.Mode.AppBooting, CurMode);
        }

        [Fact]
        public void QrCodeNotWorking_ModeOOO()
        {
            AllWorking_ModeInservice();
            // Act            
            qr.Publish(QrReaderStatus.Disconnected);

            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.OOO,
                CurMode
                );

            // Act            
            qr.Publish(QrReaderStatus.AllGood);

            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.InService,
                CurMode
                );
        }

        [Fact]
        public void ValidationMgrNotWorking_ModeOOO()
        {
            AllWorking_ModeInservice();

            offline.Publish(OfflineValidationSystemStatus.Obsolete);
            online.Publish(OnlineValidationSystemStatus.Disconnected);
            
            Assert.Equal(global::EtGate.Domain.Mode.OOO,
                CurMode
                );

            offline.Publish(OfflineValidationSystemStatus.AllGood);
            Assert.Equal(global::EtGate.Domain.Mode.InService,
                CurMode
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
                CurMode
                );

            // Act            
            gate.Publish(GateHwStatus.AllGood);

            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.InService,
                CurMode
                );
        }

        [Fact]
        public void AllWorking_ModeInservice()
        {
            // Assert
            Assert.Equal(global::EtGate.Domain.Mode.AppBooting, CurMode);

            qr.Publish(QrReaderStatus.AllGood);
            CurMode.ShouldBe(global::EtGate.Domain.Mode.AppBooting);

            offline.Publish(OfflineValidationSystemStatus.AllGood);
            CurMode.ShouldBe(global::EtGate.Domain.Mode.AppBooting);

            online.Publish(OnlineValidationSystemStatus.Disconnected);

            gate.Publish(GateHwStatus.AllGood);
            CurMode.ShouldBe(global::EtGate.Domain.Mode.InService);
        }

        [Fact]
        public void OldModeMgrDisposed()
        {
            AllWorking_ModeInservice();
            
            var modeMgrMockBak = modeMgrMock;

            offline.Publish(OfflineValidationSystemStatus.Obsolete);
            Assert.Equal(global::EtGate.Domain.Mode.OOO, CurMode);
            //
            // 
            modeMgrMockBak.Verify(m => m.Dispose(), Times.Once);            
        }

        [Fact]
        public void InSer_OOO_InSer_NewModelMgrCreated()
        {
            AllWorking_ModeInservice();

            throw new NotImplementedException();

            //var subModeMgr = CurModeMgr;
            //offline.Publish(OfflineValidationSystemStatus.Obsolete);
            //CurMode.ShouldBe(global::EtGate.Domain.Mode.OOO);
            //offline.Publish(OfflineValidationSystemStatus.AllGood);
            //CurMode.ShouldBe(global::EtGate.Domain.Mode.InService);
            //CurModeMgr.ShouldNotBeSameAs(subModeMgr);

        }
    }
}
