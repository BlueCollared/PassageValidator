namespace EtGate.Domain.Services.Gate;

public class PassageMgr : IPassageManager
{
    private readonly IPassageController controller;

    public PassageMgr()
    {
        //throw new NotImplementedException();
    }

    public PassageMgr(IPassageController controller)
    {
        this.controller = controller;
    }

    //public ObsIdleEvents IdleStatusObservable => throw new NotImplementedException();

    public ObsAuthEvents Authorize(string ticketId, int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}