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
// It is possible that around the time authroization is submitted, intrusion gets detected.
// It is its job to suppress raising the Intrusion (by first making sure that Intrusion has really removed)
public interface IPassageController
{
    ObsAuthEvents PassageStatusObservable { get; }

    // returns true if the request is accepted    
    bool Authorize(int nAuthorizations);
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