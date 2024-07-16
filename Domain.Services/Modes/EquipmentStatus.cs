namespace Domain.Services.Modes
{
    public enum ModuleStatus { Good, Bad, Unknown };
    public record EquipmentStatus(
        ModuleStatus GateHardware = ModuleStatus.Unknown,
        ModuleStatus QrEntry = ModuleStatus.Unknown,
        ModuleStatus QrExit = ModuleStatus.Unknown,
        ModuleStatus ValidationAPI = ModuleStatus.Unknown,
        ModuleStatus OfflineDataFresh = ModuleStatus.Unknown,
        bool Emergency = false
        );
}