namespace Peripherals
{
    public interface IPeripheral
    {
        // start the module. Will fail (return false), if the necessary configuration is not present, or some required dll couldn't be loaded
        bool Start();

        // do the cleanup stuff. Once "Stop"ped, no guarentee that it can be "Start"ed
        void Stop();
    }
}
