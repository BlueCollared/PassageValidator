global using RawEventsInNominalMode = OneOf.OneOf<
    EtGate.Devices.Interfaces.Gate.Intrusion,
    EtGate.Devices.Interfaces.Gate.Fraud,
    EtGate.Devices.Interfaces.Gate.OpenDoor,
    EtGate.Devices.Interfaces.Gate.WaitForAuthroization,
    EtGate.Devices.Interfaces.Gate.CloseDoor>;

using Domain.Peripherals.Passage;
using EtGate.Domain.Services.Gate;
using IFS2.Equipment.DriverInterface;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Devices.Interfaces.Gate;

abstract public class GateControllerBase : StatusStreamBase<GateHwStatus>, IGateController
{
    readonly protected ReplaySubject<GateHwStatus> GateStatusSubject = new();
    public IObservable<GateHwStatus> GateStatusObservable => GateStatusSubject.AsObservable();

    readonly protected ReplaySubject<RawEventsInNominalMode> RawEventsInNominalModeSubject = new();
    public IObservable<RawEventsInNominalMode> PassageStatusObservable => RawEventsInNominalModeSubject.AsObservable();

    abstract public bool Authorize(int nAuthorizations);

    abstract public bool Reboot(bool bHardboot);

    abstract public void SetMode(eSideOperatingModeGate entry, eSideOperatingModeGate exit, DoorsMode doorsMode);

    abstract public void SetOpMode(OpMode mode);
}
