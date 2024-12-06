// Gate might be offering Free Mode in one or both sides. So, passages can be done w/o authorizations
namespace EtGate.Domain.Passage.IdleEvts;

public record Intrusion(
    bool bEntry
    );

public record Fraud(
    bool bEntry);

public record OpenDoor(
    bool bEntry // This value is bounced back, so that the client doesn't has to do any processing. may think of removing it
    );

public record PassengerSteppedBack(
    bool bEntry // This value is bounced back, so that the client doesn't has to do any processing. may think of removing it    
    );

public record PassageTimeout(
    bool bEntry
    );

public record PassageDone(
    bool bEntry
    );