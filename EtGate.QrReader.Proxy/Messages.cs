using Domain.Peripherals.Qr;

namespace EtGate.QrReader.Proxy
{
    public class ViewModel
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
    
    public class StartReq
    { }

    public class StartResp
    {
        public bool x { get; set; }
    }

    public class StopReq
    { }

    public class StopResp
    { }

    public class StopDetectingReq
    { }

    public class StopDetectingResp
    { }
}