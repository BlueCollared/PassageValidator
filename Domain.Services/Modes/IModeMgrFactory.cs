using Domain.Services.InService;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;

namespace Domain.Services.Modes
{
    public interface IModeMgrFactory
    {
        ISubModeMgr Create(Mode mode);
    }

    public class ModeMgrFactory : IModeMgrFactory
    {
        private readonly IQrReaderMgr qrReaderMgr;
        private readonly ValidationMgr validationMgr;

        public ModeMgrFactory(IQrReaderMgr qrReaderMgr, ValidationMgr validationMgr)
        {
            this.qrReaderMgr = qrReaderMgr;
            this.validationMgr = validationMgr;
        }

        ISubModeMgr IModeMgrFactory.Create(Mode mode)
        {
            switch (mode)
            {
                case Mode.InService:
                    return new InServiceMgr(validationMgr, new PassageMgr(), qrReaderMgr);                    
                default:
                    return new DoNothingModeMgr(mode);                    
            }
        }
    }
}