namespace RecordsRemoteListener
{
    public class ChangeMode : BaseRequest
    {
        public int mode;
    }

    public class ChangeModeResponse : BaseResponse
    {
        public bool result;
    }
}