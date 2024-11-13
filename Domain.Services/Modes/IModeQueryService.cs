using Domain.Services.InService;

namespace Domain.Services.Modes;

public interface IModeQueryService
{
    IObservable<(Mode, ISubModeMgr)> EqptModeObservable { get; }
}
