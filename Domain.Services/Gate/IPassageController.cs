using Domain.Peripherals.Passage;

namespace EtGate.Domain.Services.Gate;

// It is possible that around the time authroization is submitted, intrusion gets detected.
// It is its job to suppress raising the Intrusion (by first making sure that Intrusion has really removed)
public interface IPassageController
{
    IObservable<GateHwStatus> GateStatusObservable { get; }

    // TODO: not yet sure that whether I should be responsible for tracking the individual passages (by also bouncing back the {`ticketId`, authorizationId})
    // or should simply leave it to the client.
    // can change in later versions
    ObsAuthEvents PassageStatusObservable { get; }

    // returns true if the request is accepted    
    bool Authorize(int nAuthorizations);    
}