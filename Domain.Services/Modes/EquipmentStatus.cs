using EtGate.Domain;
using EtGate.Domain.Peripherals.Passage;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;

namespace Domain.Services.Modes
{
    //abstract public record SWModuleStatus
    //{
    //    public abstract SWModuleStatus defStatus { get; }
    //}

    //public record QrReaderStatus_ 
    //{        
    //    public QrReaderStatus_ UpdateStatus(QrReaderStatus newStatus) => this with { IsKnown = true, Status = newStatus };

    //    public bool IsKnown { get; init; } = false;
    //    public QrReaderStatus Status { get; init; } //= QrReaderStatus.Disconnected;
    //}

    //public record ValidationSystemStatus_
    //{        
    //    public ValidationSystemStatus_ UpdateStatus(ValidationSystemStatus newStatus) => this with { IsKnown = true, Status = newStatus };

    //    public bool IsKnown { get; init; } = false;
    //    public ValidationSystemStatus Status { get; init; }//= ValidationSystemStatus.Default;
    //}

    //public record GateHwStatus_
    //{
    //    public GateHwStatus_ UpdateStatus(GateHwStatus newStatus) => this with { IsKnown = true, Status = newStatus };

    //    public bool IsKnown { get; init; } = false;
    //    public GateHwStatus Status { get; init; }//= ValidationSystemStatus.Default;
    //}

    public record EquipmentStatus(
        QrReaderStatus QrEntry,// = ModuleStatus.Unknown,        
                               //ValidationSystemStatus ValidationAPI,//; = ModuleStatus.Unknown,        
        OfflineValidationSystemStatus offline,
        OnlineValidationSystemStatus online,
        GateHwStatus gateStatus,
        bool bMaintAsked,
        OpMode modeAsked
        );
}