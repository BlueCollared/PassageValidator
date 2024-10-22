using EtGate.Domain.Services.Gate.Functions;

namespace EtGate.Domain.Services.Gate;

public class GateConnectedSession : IDisposable
{
    //private readonly IGateController gateController;

    public ClockSynchronizer dateMgr { get; private set; } // Similarly other managers (one per functionality e.g. FirmwareUpgrader)
    public IPassageController passageMgr { get; private set; }

    public GateConnectedSession(Func<ClockSynchronizer> factoryClockSynch)
    {
        //this.gateController = gateController;

        dateMgr = factoryClockSynch?.Invoke();
    }

    public void Dispose()
    {
        try
        {
            dateMgr?.Cancel();
            //passageMgr?.Dispose();
        }
        catch { }
    }
}