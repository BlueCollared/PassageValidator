using Domain.Peripherals.Passage;
using Equipment.Core.Message;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Gate;
using Moq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Domain.Tests
{
    public class GateMgrTests
    {
        class GateSatatusMgr //: IMessageSubscriber<GateHwStatus>
        {
            public GateSatatusMgr() : base()
            {
            }
            public Subject<GateHwStatus> statusSubject = new Subject<GateHwStatus>();

            public IObservable<GateHwStatus> Messages => statusSubject.AsObservable();
            //public IObservable<GateHwStatus> statusObservable => statusSubject.AsObservable();
        }

        [Fact]
        public void GateConnected_CallsGetDate()
        {
            GateSatatusMgr gateSatatusMgrMock = new GateSatatusMgr();
            var mockGateController = new Mock<IDeviceDate>();
            
            GateMgr gateMgr = new GateMgr(mockGateController.Object, new Mock<IDeviceStatusSubscriber<GateHwStatus>>().Object,
                new GateMgr.Config {  ClockSynchronizerConfig = new() { interval = TimeSpan.FromSeconds(60) } });
            gateSatatusMgrMock.statusSubject.OnNext(GateHwStatus.AllGood);
            mockGateController.Verify(x => x.GetDate(), Times.Once);
        }
    }
}
