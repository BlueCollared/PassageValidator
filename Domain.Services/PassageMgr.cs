namespace Domain.InService;

public enum PassageEvtTypes
{
    EntryPassed,
    ExitPassed,
    EntryPassageTimeout,
    ExitPassageTimeout,
    EntryIntrusion,
    ExitIntrusion
}

public class PassageDeviceMgr : IPassageManager
{
    public ObsAuthEvents IdleStatusObservable => throw new NotImplementedException();

    public ObsAuthEvents Authorize(string ticketId, int nAuthorizations)
    {
        throw new NotImplementedException();
    }
}
