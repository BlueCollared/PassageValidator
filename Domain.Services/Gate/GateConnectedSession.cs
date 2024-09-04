namespace EtGate.Domain.Services.Gate;

public class GateConnectedSession : IDisposable
{
    private readonly IGateController gateController;

    public ClockSynchronizer dateMgr { get; private set; } // Similarly other managers (one per functionality e.g. FirmwareUpgrader)
    public IPassageManager passageMgr { get; private set; }

    public GateConnectedSession(IGateController gateController)
    {
        this.gateController = gateController;
        
        dateMgr = new ClockSynchronizer(gateController,
            () => DateTimeOffset.Now,
            null,
            new ClockSynchronizer.Config(TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(200), TimeSpan.MaxValue)
            );
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