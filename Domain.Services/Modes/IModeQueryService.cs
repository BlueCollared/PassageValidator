using Domain.Services.InService;
using EtGate.Domain;

namespace Domain.Services.Modes;

public interface IModeQueryService
{
    IObservable<(Mode, ISubModeMgr)> EqptModeObservable { get; }
}
