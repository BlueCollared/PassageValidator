using Domain.Peripherals.Passage;
using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate;
using IFS2.Equipment.DriverInterface;

namespace EtGate.Devices.IER;

public class PasageDeviceController : PassageControllerBase
{
//    private readonly Subject<ZoneEvent> _zoneEventsSubject = new Subject<ZoneEvent>();


    public IObservable<GateHwStatus> GateStatusObservable => throw new NotImplementedException();

    public override bool Authorize(int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public override bool Reboot(bool bHardboot)
    {
        throw new NotImplementedException();
    }

    public override void SetMode(eSideOperatingModeGate entry, eSideOperatingModeGate exit, DoorsMode doorsMode)
    {
        throw new NotImplementedException();
    }

    public override void SetOpMode(OpMode mode)
    {
        throw new NotImplementedException();
    }
}
