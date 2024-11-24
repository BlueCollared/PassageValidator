using EtGate.Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Qr
{
    public interface IQrReaderMgr
    {
        const string Entry = nameof(Entry);
        const string Exit = nameof(Exit);
        const string Both = nameof(Both);

        Task<(string ReaderMnemonic, QrCodeInfo QrCodeInfo)> StartDetecting(string qrs, CancellationToken cancelToken);
    }
}