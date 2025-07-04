﻿using EtGate.Domain;

namespace Domain.Services;

public interface ITicketValidationService
{
    bool ValidateQrCode(Ticket ticket);
    void UpdatePassengerStatus(bool hasCrossedZoneB);
    //IPassageController PassageMgr { get; }
}