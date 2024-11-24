using EtGate.Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Qr;

// We assume that: once it detects a media, it stops, and `StartDetecting` needs to be called again
public interface IQrReaderController
{
    Task<QrCodeInfo?> StartDetecting(CancellationToken cancellationToken); // Returns null if cancelled    
}
