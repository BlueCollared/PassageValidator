namespace Domain.Peripherals.Qr
{
    public interface IQrReaderFactory
    {
        IQrReader CreateReader(string parsToBeDecided);
    }
}