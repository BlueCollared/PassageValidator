using Domain.Services.InService;
using EtGate.Domain;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using Equipment.Core.Message;

namespace Domain.Services.Modes;

public interface ISubModeMgrFactory
{
    ISubModeMgr Create(Mode mode);
}

public class SubModeMgrFactory : ISubModeMgrFactory
{
    private readonly IQrReaderMgr qrReaderMgr;
    private readonly ValidationMgr validationMgr;
    private readonly IGateInServiceController passageMgr;
    private readonly DeviceStatusSubscriber<ActiveFunctionalities> actFnSubs;

    public SubModeMgrFactory(IQrReaderMgr qrReaderMgr, 
        ValidationMgr validationMgr, 
        IGateInServiceController passageMgr,
        DeviceStatusSubscriber<ActiveFunctionalities> actFnSubs)
    {
        this.qrReaderMgr = qrReaderMgr;
        this.validationMgr = validationMgr;
        this.passageMgr = passageMgr;
        this.actFnSubs = actFnSubs;
    }

    public ISubModeMgr Create(Mode mode)
    {
        switch (mode)
        {
            case Mode.InService:
                return new InServiceMgr(validationMgr, 
                    //passageMgr, 
                    qrReaderMgr,
                    actFnSubs);
            default:
                return new DoNothingModeMgr(mode);                    
        }
    }
}