namespace EtGate.Devices.Interfaces.Gate
{
    // It is possible that around the time authroization is submitted, intrusion gets detected.
    // It is its job to suppress raising the Intrusion (by first making sure that Intrusion has really removed)

    public interface IPassageController
    {
        IObservable<RawEventsInNominalMode> PassageStatusObservable { get; }

        // returns true if the request is accepted    
        bool Authorize(int nAuthorizations);
    }
}
