using Domain;

namespace EtGate.UI.ViewModels
{
    public interface IModeViewModelFactory
    {
        ModeViewModel Create(Mode mode, bool bPrimary, bool bEntry);
    }
}