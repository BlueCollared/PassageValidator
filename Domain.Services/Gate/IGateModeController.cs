using Domain.Peripherals.Passage;
using IFS2.Equipment.DriverInterface;
using LanguageExt;

namespace EtGate.Domain.Services.Gate;

// will be used by ModeService
public interface IGateModeController
{
    bool SetEmergency();
    bool SetMaintenance();
    bool SetNormalMode(Option<SideOperatingModes> entry, Option<SideOperatingModes> exit);
    

    IObservable<GateHwStatus> GateStatusObservable { get; }
}