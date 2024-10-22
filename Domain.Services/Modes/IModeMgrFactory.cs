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
        private readonly IPassageController passageMgr;

        public ModeMgrFactory(IQrReaderMgr qrReaderMgr, ValidationMgr validationMgr, IPassageController passageMgr)
        {
            this.qrReaderMgr = qrReaderMgr;
            this.validationMgr = validationMgr;
            this.passageMgr = passageMgr;
        }

        ISubModeMgr IModeMgrFactory.Create(Mode mode)
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