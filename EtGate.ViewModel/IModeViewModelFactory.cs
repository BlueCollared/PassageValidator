using Domain;

namespace EtGate.UI.ViewModels
{
    public interface IModeViewModelFactory
    {
        ModeViewModel Create(Mode mode, global::Domain.Services.InService.ISubModeMgr subModeMgr, bool bPrimary, bool bEntry);
    }
}