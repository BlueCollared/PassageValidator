using Domain.Peripherals.Passage;
using IFS2.Equipment.DriverInterface;

namespace EtGate.Domain.Services.Gate;

public enum OpMode
{
    /// <summary>
    /// Normal operation mode.
    /// </summary>
    Normal,
    /// <summary>
    /// Maintenance operation mode.
    /// </summary>
    Maintenance,
    /// <summary>
    /// Emergency operation mode.
    /// </summary>
    Emergency
}

// will be used by ModeService
public interface IGateModeController
{ 
    void SetMode(eSideOperatingModeGate entry, // Closed/Controlled/Free
    eSideOperatingModeGate exit, // Closed/Controlled/Free
    DoorsMode doorsMode
    );

    void SetOpMode(OpMode mode);

    IObservable<GateHwStatus> GateStatusObservable { get; }
}