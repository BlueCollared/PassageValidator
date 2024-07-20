namespace Peripherals
{
    public interface IPeripheral
    {
        // start the software module (the peripheral would be started (or already has started) in a separate thread). Will fail (return false), if the necessary configuration is not present, or some required dll couldn't be loaded
        // one more use could have been: it starts pumping events only after Start is called.
        // TODO: See if this method can be scrapped.
        bool Start();

        // do the cleanup stuff. Once "Stop"ped, no guarentee that it can be "Start"ed
        void Stop();
    }
}
