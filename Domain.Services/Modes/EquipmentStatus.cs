namespace Domain.Services.Modes
{
    public enum Mode { InService, OOO, Emergency};
    public record EquipmentStatus(
        bool? GateHardware,
        bool? QrEntry,
        bool? QrExit,
        bool? ValidationAPI,
        bool? OfflineDataFresh,
        bool Emergency = false
        )
    { 
        public Mode InService
        {
            get
            {
                return Mode.InService;
            }
        }
    }    
}
