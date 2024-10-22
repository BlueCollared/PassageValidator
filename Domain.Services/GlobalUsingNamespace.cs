global using EventInNominalMode = OneOf.OneOf<
    EtGate.Domain.Services.Gate.IntrusionX,
    EtGate.Domain.Services.Gate.Fraud,
    EtGate.Domain.Services.Gate.OpenDoor,
    EtGate.Domain.Services.Gate.WaitForAuthroization,
    EtGate.Domain.Services.Gate.CloseDoor>;

//global using ObsAuthEvents = System.IObservable<OneOf.OneOf<
//    EtGate.Domain.Passage.PassageEvts.Intrusion,
//    EtGate.Domain.Passage.PassageEvts.Fraud,
//    EtGate.Domain.Passage.PassageEvts.OpenDoor,
//    EtGate.Domain.Passage.PassageEvts.PassageTimeout,
//    EtGate.Domain.Passage.PassageEvts.AuthroizedPassengerSteppedBack,
//    EtGate.Domain.Passage.PassageEvts.PassageDone>>;
