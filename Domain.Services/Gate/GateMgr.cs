using Domain.Peripherals.Passage;

namespace EtGate.Domain.Services.Gate
{
    public class GateMgr
    {
        public IGateController gateController { get; private set;}
        private readonly IDeviceStatus<GateHwStatus> statusMgr;

        public IObservable<GateHwStatus> StatusStream => statusMgr.statusObservable;

        GateConnectedSession session;

        public GateMgr(IGateController gateController,
            IDeviceStatus<GateHwStatus> statusMgr)
        {
            this.gateController = gateController;
            this.statusMgr = statusMgr;

            statusMgr.statusObservable.Subscribe(status => { 
                if (!status.bConnected)
                    session?.Dispose();
                else
                {
                    if (session == null)
                        session = new GateConnectedSession(this.gateController);                   
                }
            });
        }
    }
}
