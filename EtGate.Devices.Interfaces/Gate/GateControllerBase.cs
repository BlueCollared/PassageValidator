global using RawEventsInNominalMode = OneOf.OneOf<
    EtGate.Devices.Interfaces.Gate.Intrusion,
    EtGate.Devices.Interfaces.Gate.Fraud,
    EtGate.Devices.Interfaces.Gate.OpenDoor,
    EtGate.Devices.Interfaces.Gate.WaitForAuthroization,
    EtGate.Devices.Interfaces.Gate.CloseDoor>;

using Domain.Peripherals.Passage;
using IFS2.Equipment.DriverInterface;
using LanguageExt;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Devices.Interfaces.Gate;

abstract public class GateControllerBase : StatusStreamBase<GateHwStatus>, IGateController
{
    readonly protected ReplaySubject<GateHwStatus> GateStatusSubject = new();
    public IObservable<GateHwStatus> GateStatusObservable => GateStatusSubject.AsObservable();

    readonly protected ReplaySubject<RawEventsInNominalMode> RawEventsInNominalModeSubject = new();
    public IObservable<RawEventsInNominalMode> PassageStatusObservable => RawEventsInNominalModeSubject.AsObservable();
    
    public abstract bool SetEmergency();
    public abstract bool SetMaintenance();
    public abstract bool SetNormalMode(Option<SideOperatingModes> entry, Option<SideOperatingModes> exit);
    public abstract bool Reboot(bool bHardboot);
    public abstract bool Authorize(int nAuthorizations);
}
