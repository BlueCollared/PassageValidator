using EtGate.Domain;

namespace Domain.Services.Modes;

public interface IModeManager
{
    Mode CurMode { get; }
    OpMode ModeDemanded { get; set; }

    Task SwitchOutMaintenance();
    Task SwitchToMaintenance();
}
