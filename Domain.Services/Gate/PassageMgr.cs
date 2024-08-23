namespace EtGate.Domain.Services.Gate;

public class PassageDeviceMgr : IPassageManager
{
    public ObsIdleEvents IdleStatusObservable => throw new NotImplementedException();

    public ObsAuthEvents Authorize(string ticketId, int nAuthorizations)
    {
        throw new NotImplementedException();
    }
}