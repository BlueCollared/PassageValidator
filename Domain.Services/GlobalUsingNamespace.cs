global using EventInNominalMode = OneOf.OneOf<
    EtGate.Domain.Services.Gate.IntrusionX,
    EtGate.Domain.Services.Gate.Fraud,
    EtGate.Domain.Services.Gate.OpenDoor,
    EtGate.Domain.Services.Gate.WaitForAuthroization,
    EtGate.Domain.Services.Gate.CloseDoor>;