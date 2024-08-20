namespace EtGate.Domain.Passage.PassageEvts;

public record SubTicket (int ticketId, int seqNumInThisTicket);

public record Intrusion(
    bool bEntry,
    List<SubTicket> remainingQueue);

public record Fraud(
    bool bEntry,
    List<SubTicket> remainingQueue);

public record PassageInProgress(
    SubTicket ticket,
    bool bEntry, // This value is bounced back, so that the client doesn't has to do any processing. may think of removing it
    List<SubTicket> remainingQueue);

public record AuthroizedPassengerSteppedBack(
    SubTicket ticket,
    bool bEntry, // This value is bounced back, so that the client doesn't has to do any processing. may think of removing it
    List<SubTicket> remainingQueue
    );

public record PassageTimeout(
    SubTicket ticket,
    bool bEntry,
    List<SubTicket> remainingQueue
    );

public record PassageDone(
    SubTicket ticket,
    bool bEntry,
    List<SubTicket> remainingQueue
    );