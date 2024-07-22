using Domain.Peripherals.Qr;

namespace EtGate.QrReader.Proxy
{
    //public class QrRdrStatusChanged : INotification
    //{
    //    public QrReaderStatus status = new(true, "1.1", false);
    //}
    public class ViewModel //: TypeHolding
    {
        public StartDetectingResp StartDetectingResp { get; set; }
        public StartResp StartResp { get; set; }

        public QrReaderStatus QrReaderStatus { get; set; }
    }

    public class StartDetectingReq
    { }

    public class StartDetectingResp
    {
        public bool x { get; set; }
    }
    
    public class StartReq //: IRequest
    { }

    public class StartResp //: IResponse
    {
        public bool x { get; set; }
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