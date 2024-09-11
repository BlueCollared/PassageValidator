using IFS2.Equipment.HardwareInterface.IERPLCManager;
using System.Collections.Immutable;

namespace EtGate.IER
{
    // TODO: necessary implementation to avoid the use of heap-allocated memory
    public record struct IERStatus(
        bool bUserProcessing = false,
        bool bIsEmergency = false,
        bool bIsFraudOrIntrusion = false,
        bool bIsTimeout = false,
        bool bTechnicalError = false,
        bool bEmergency = false,
        bool bMaintenance = false,
        bool bSideModesAndDoorModesForcedByInputs = false,
        int nAuthrisationsFromEntrance = 0,
        int nAuthrisationsFromExit = 0,
        ImmutableArray<eInfractions> Infractions = default,
        ImmutableArray<ePassageTimeouts> Timeouts = default,
        ImmutableDictionary<int, string> Errors = default
        )
    { 
    };
}
