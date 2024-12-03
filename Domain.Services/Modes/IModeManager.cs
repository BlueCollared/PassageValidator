using EtGate.Domain;

namespace Domain.Services.Modes;

public interface IModeManager
{    
    OpMode ModeDemanded { get; set; }

    Task SwitchOutMaintenance();
    Task SwitchToMaintenance();
}
