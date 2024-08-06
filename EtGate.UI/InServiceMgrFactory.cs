//using Domain.InService;
//using Domain.Services.InService;
//using EtGate.Domain.Services.Qr;
//using EtGate.Domain.Services.Validation;

//namespace EtGate.UI
//{
//    public class InServiceMgrFactory //: IInServiceMgrFactory
//    {
//        private ValidationMgr validationMgr;
//        private IPassageManager passage;
//        private IQrReaderMgr qrReader;

//        public IInServiceMgr Create()
//        {
//            return new InServiceMgr(validationMgr, passage, qrReader);
//        }

//        public InServiceMgrFactory(ValidationMgr validationMgr,
//            IPassageManager passage,
//            IQrReaderMgr qrReader)
//        {            
//            this.validationMgr = validationMgr;
//            this.passage = passage;
//            this.qrReader = qrReader;
//        }
//    }
//}
