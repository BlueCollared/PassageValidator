using Domain.Services.InService;
using EtGate.Domain;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;

namespace Domain.Services.Modes
{
    public interface ISubModeMgrFactory
    {
        ISubModeMgr Create(Mode mode);
    }

    public class ModeMgrFactory : ISubModeMgrFactory
    {
        private readonly IQrReaderMgr qrReaderMgr;
        private readonly ValidationMgr validationMgr;
        private readonly IGateInServiceController passageMgr;

        public ModeMgrFactory(IQrReaderMgr qrReaderMgr, ValidationMgr validationMgr, IGateInServiceController passageMgr)
        {
            this.qrReaderMgr = qrReaderMgr;
            this.validationMgr = validationMgr;
            this.passageMgr = passageMgr;
        }

        ISubModeMgr ISubModeMgrFactory.Create(Mode mode)
        {
            switch (mode)
            {
                case Mode.InService:
                    return new InServiceMgr(validationMgr, passageMgr, qrReaderMgr);
                default:
                    return new DoNothingModeMgr(mode);                    
            }
        }
    }
}