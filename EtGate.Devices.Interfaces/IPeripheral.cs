namespace Peripherals
{
    public interface IPeripheral
    {
        // start the software module (the peripheral would be started (or already has started) in a separate thread). Will fail (return false), if the necessary configuration is not present, or some required dll couldn't be loaded
        // one more use could have been: it starts pumping events only after Start is called. Yes, it is important
        // else, we loose events in the beginning. ReplaySubject doesn't help as its default capacity is 1.even if we specify a large capacity, we are still making unnecessary compromise
        // only thing is who will call Start? Orchestrator or the Domain service?
        bool Start();

        // do the cleanup stuff. Once "Stop"ped, no guarentee that it can be "Start"ed
        void Stop();
    }
}
