namespace Domain.Peripherals
{
    public interface IPeripheral
    {
        bool Init();
        bool Stop();
    }
}
