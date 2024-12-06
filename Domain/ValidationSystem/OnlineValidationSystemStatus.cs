namespace EtGate.Domain.ValidationSystem
{
    public record OnlineValidationSystemStatus(bool bConnected) //: ModuleStatus
    {
        public bool IsAvailable => bConnected;

        public static OnlineValidationSystemStatus Disconnected => new OnlineValidationSystemStatus(bConnected: false);

        //public ModuleStatus defStatus => Disconnected;
    }
}
