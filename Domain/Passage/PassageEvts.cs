using IFS2.Equipment.HardwareInterface.IERPLCManager;

namespace EtGate.Domain.Passage.PassageEvts;

public record SubTicket (int ticketId, int seqNumInThisTicket);

public record Intrusion(
    bool bEntry,
    List<SubTicket> remainingQueue);

public record Fraud(
    bool bEntry,
    List<SubTicket> remainingQueue);

public record OpenDoor(
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
    ePassageTimeouts timeout
    // by the understanding so far, a timeout in the passage causes all the auhroizations to be nullified
    //List<SubTicket> remainingQueue 
    );

public record PassageDone(
    SubTicket ticket,
    bool bEntry,
    List<SubTicket> remainingQueue
    );