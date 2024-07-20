using NamedPipeHandler;

namespace EtGate.QrReader.Proxy
{
    

    public class StartDetectingReq : IRequest
    { }

    public class StartDetectingResp
    {
        public bool x;
    }
    
    public class StartReq : IRequest
    { }

    public class StartResp : IResponse
    {
        public bool x;
    }

    public class StopReq : IRequest
    { }

    public class StopResp
    { }

    public class StopDetectingReq : IRequest
    { }

    public class StopDetectingResp
    { }
}