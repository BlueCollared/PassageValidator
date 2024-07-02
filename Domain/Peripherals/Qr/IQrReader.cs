namespace Domain.Peripherals.Qr
{
    public interface IQrReader : IPeripheral
    {
        string id { get; } // typically would be "Entry"/"Exit"
    }
}
