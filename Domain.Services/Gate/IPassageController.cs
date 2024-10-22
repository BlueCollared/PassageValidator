namespace EtGate.Domain.Services.Gate;

// 2024-10-21 : NO, I THINK BELOW COMMENTS ARE BAD
// It is possible that around the time authroization is submitted, intrusion gets detected.
// It is its job to suppress raising the Intrusion (by first making sure that Intrusion has really removed)

public interface IPassageController
{
    IObservable<EventInNominalMode> InServiceEventsObservable { get; }

    // returns true if the request is accepted    
    bool Authorize(int nAuthorizations, bool bEntry);
}
