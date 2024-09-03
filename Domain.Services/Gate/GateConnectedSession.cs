using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate.Functions;

namespace EtGate.Domain.Services.Gate
{
    public class GateConnectedSession : IDisposable
    {
        private readonly IGateController gateController;

        public DateMgr dateMgr { get; private set; } // Similarly other managers (one per functionality)
        public IPassageManager passageMgr { get; private set; }

        public GateConnectedSession(IGateController gateController)
        {
            this.gateController = gateController;

            DateMgr.Config config = new();
            config.tsFrequencyToCheck = TimeSpan.FromSeconds(5 * 60);
            dateMgr = new DateMgr(gateController, config);
        }

        public void Dispose()
        {
            dateMgr?.Dispose();
            //passageMgr?.Dispose();
        }
    }
}