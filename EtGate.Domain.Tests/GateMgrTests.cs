using Domain.Peripherals.Passage;
using EtGate.Domain.Services.Gate;
using Moq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Domain.Tests
{
    public class GateMgrTests
    {
        class GateSatatusMgr : IDeviceStatus<GateHwStatus>
        {
            public GateSatatusMgr() : base()
            {
            }
            public Subject<GateHwStatus> statusSubject = new Subject<GateHwStatus>();
            public override IObservable<GateHwStatus> statusObservable => statusSubject.AsObservable();
        }

        [Fact]
        public void GateConnected_CallsGetDate()
        {
            GateSatatusMgr gateSatatusMgrMock = new GateSatatusMgr();
            var mockGateController = new Mock<IGateController>();
            
            GateMgr gateMgr = new GateMgr(mockGateController.Object, gateSatatusMgrMock, 
                new GateMgr.Config {  ClockSynchronizerConfig = new() { interval = TimeSpan.FromSeconds(60) } });
            gateSatatusMgrMock.statusSubject.OnNext(new GateHwStatus(true, eDoorsStatesMachine.NOMINAL));
            mockGateController.Verify(x => x.GetDate(), Times.Once);


        }
    }
}
