namespace EtGate.Domain
{
    public class Ticket
    {
        public string QrCode { get; set; }
        public bool IsValid { get; set; }
    }

    public class PassengerStatus
    {
        public bool HasCrossedZoneB { get; set; }
        public bool FlapOpened { get; set; }
    }

    public class GateStatus
    {
        public bool IsInService { get; set; }
        public bool IsQrReaderWorking { get; set; }
        public bool IsFlapControllerWorking { get; set; }
    }
}