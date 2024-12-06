using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain.Services.Modes;
using Microsoft.Reactive.Testing;
using Moq;
using Shouldly;

namespace EtGate.Domain.Tests;

public class ModeFacadeTests
{
    ModeFacade sut;
    DeviceStatusSubscriberTest<(Mode, bool)> modeSubscriber = new();
    PeripheralStatuses_Test p = PeripheralStatuses_Test.ForTests();
    TestScheduler scheduler = new TestScheduler();
    int timeOut = 20;
    DeviceStatusBusTest<(Mode, ISubModeMgr)> deviceStatus = new();
    Mock<ISubModeMgrFactory> modeMgrFactoryMock = new();
    Mock<ISubModeMgr> modeMgrMock;

    public ModeFacadeTests()
    {
        modeMgrFactoryMock
                .Setup(f => f.Create(It.IsAny<global::EtGate.Domain.Mode>()))
                .Returns(() =>
                {
                    modeMgrMock = new Mock<ISubModeMgr>();
                    return modeMgrMock.Object;
                });

    }

    [Fact]
    public void OldModeMgrDisposed()
    {
        sut = new ModeFacade(p, modeMgrFactoryMock.Object, deviceStatus, null, scheduler, timeOut);

        modeSubscriber.Publish((Mode.AppBooting, true));
        var bak = modeMgrMock;
        //p.SimulateStateChange();
        scheduler.AdvanceBy(TimeSpan.FromSeconds(timeOut).Ticks);

        bak.Verify(m => m.Dispose(), Times.Once);
    }

    [Fact]
    public void InSer_OOO_InSer_NewModelMgrCreated()
    {
        sut = new ModeFacade(PeripheralStatuses_Test.AllGood(), modeMgrFactoryMock.Object, deviceStatus, null, scheduler, timeOut);
        //AllWorking_ModeInservice();

        //throw new NotImplementedException();

        //var subModeMgr = CurModeMgr;
        //offline.Publish(OfflineValidationSystemStatus.Obsolete);
        //CurMode.ShouldBe(global::EtGate.Domain.Mode.OOO);
        //offline.Publish(OfflineValidationSystemStatus.AllGood);
        deviceStatus.curStatus.Item1.ShouldBe(global::EtGate.Domain.Mode.InService);
        //CurModeMgr.ShouldNotBeSameAs(subModeMgr);

    }

}
