using Domain.Peripherals.Passage;
using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate.Functions;

namespace EtGate.Domain.Services.Gate
{
    public class GateMgr
    {
        private readonly IGateController gateController;
        private readonly IDeviceStatus<GateHwStatus> statusMgr;

        DateMgr dateMgr; // Similarly other managers (one per functionality)
        public GateMgr(IGateController gateController,
            IDeviceStatus<GateHwStatus> statusMgr)
        {
            this.gateController = gateController;
            this.statusMgr = statusMgr;

            statusMgr.statusObservable.Subscribe(status => { 
                if (!status.bConnected)
                {
                    if (dateMgr != null)
                    {
                        dateMgr.Dispose();
                        dateMgr = null;
                    }
                }
                else
                {
                    if (dateMgr == null)
                    {
                        DateMgr.Config config = new();
                        config.tsFrequencyToCheck = TimeSpan.FromSeconds(5 * 60);
                        dateMgr = new DateMgr(gateController, config);
                    }
                }
            });
        }
    }
}
