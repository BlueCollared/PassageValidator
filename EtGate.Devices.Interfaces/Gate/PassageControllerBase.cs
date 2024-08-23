using Domain.Peripherals.Passage;
using EtGate.Domain.Services.Gate;
using IFS2.Equipment.DriverInterface;

namespace EtGate.Devices.Interfaces.Gate;

abstract public class PassageControllerBase : StatusStreamBase<GateHwStatus>, IPassageControllerEx
{    
    public IObservable<GateHwStatus> GateStatusObservable => throw new NotImplementedException();    

    //public 
        RawEventsInNominalMode RawEvtsInIdleModeObservable => throw new NotImplementedException();

    public ObsAuthEvents PassageStatusObservable => throw new NotImplementedException();

    abstract public bool Authorize(int nAuthorizations);

    abstract public bool Reboot(bool bHardboot);

    abstract public void SetMode(eSideOperatingModeGate entry, eSideOperatingModeGate exit, DoorsMode doorsMode);

    abstract public void SetOpMode(OpMode mode);
}
