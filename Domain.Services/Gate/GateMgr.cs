using Domain.Peripherals.Passage;
using EtGate.Devices.Interfaces.Gate;

namespace EtGate.Domain.Services.Gate
{
    public class GateMgr
    {
        private readonly IGateController gateController;
        private readonly IDeviceStatus<GateHwStatus> statusMgr;

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
