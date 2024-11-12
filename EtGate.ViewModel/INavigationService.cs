using EtGate.UI.ViewModels;

namespace EtGate.UI;

public interface INavigationService
{    
    void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase;
    void GoBack();
}
