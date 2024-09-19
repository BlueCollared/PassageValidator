using Domain.Peripherals.Passage;
using LanguageExt;

namespace EtGate.Domain.Services.Gate;

// will be used by ModeService
public interface IGateModeController
{
    bool SetEmergency();
    bool SetMaintenance();
    bool SetNormalMode(Option<SideOperatingMode> entry, Option<SideOperatingMode> exit);
    

    IObservable<GateHwStatus> GateStatusObservable { get; }
}