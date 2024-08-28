namespace RecordsRemoteListener
{
    abstract public class BaseRequest
    {
        // ANUJ: since we use inheritance, we may add a field like this:
        public int reqId;
    }

    abstract public class BaseResponse
    {
        public int respId; // can be used to match with the reqId
    }
}