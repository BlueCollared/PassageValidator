namespace EtGate.QrReader.Proxy
{
    //public class QrRdrStatusChanged : INotification
    //{
    //    public QrReaderStatus status = new(true, "1.1", false);
    //}

    public class StartDetectingReq //: IRequest
    { }

    public class StartDetectingResp
    {
        public bool x;
    }
    
    public class StartReq //: IRequest
    { }

    public class StartResp //: IResponse
    {
        public bool x;
    }

    public class StopReq //: IRequest
    { }

    public class StopResp
    { }

    public class StopDetectingReq //: IRequest
    { }

    public class StopDetectingResp
    { }
}