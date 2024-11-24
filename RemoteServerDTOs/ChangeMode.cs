using EtGate.Domain;

namespace RecordsRemoteListener
{
    public class ChangeMode : BaseRequest
    {
        public OpMode mode;
    }

    public class ChangeModeResponse : BaseResponse
    {
        public bool result;
    }
}