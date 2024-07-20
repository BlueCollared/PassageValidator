namespace Domain.Peripherals.Qr
{
    public record QrReaderStatus (bool bConnected, string firmwareVersion, bool bScanning);
    
}