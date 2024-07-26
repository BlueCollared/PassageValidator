using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;

namespace Domain.Services.Modes
{
    public record QrReaderStatus_
    {        
        public QrReaderStatus_ UpdateStatus(QrReaderStatus newStatus) => this with { IsKnown = true, Status = newStatus };

        public bool IsKnown { get; init; } = false;
        public QrReaderStatus Status { get; init; } //= QrReaderStatus.Disconnected;
    }

    public record ValidationSystemStatus_
    {        
        public ValidationSystemStatus_ UpdateStatus(ValidationSystemStatus newStatus) => this with { IsKnown = true, Status = newStatus };

        public bool IsKnown { get; init; } = false;
        public ValidationSystemStatus Status { get; init; }//= ValidationSystemStatus.Default;
    }
    
    public record EquipmentStatus(        
        QrReaderStatus_ QrEntry// = ModuleStatus.Unknown,        
        , ValidationSystemStatus_ ValidationAPI//; = ModuleStatus.Unknown,        
        , bool Emergency = false
        );
}