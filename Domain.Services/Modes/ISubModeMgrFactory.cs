using Domain.Services.InService;
using EtGate.Domain;

namespace Domain.Services.Modes;

public interface ISubModeMgrFactory
{
    ISubModeMgr Create(Mode mode);
}
