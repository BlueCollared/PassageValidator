using EtGate.UI.ViewModels;

namespace EtGate.UI;

public interface INavigationService
{
    //ContentControl host { get; set; }
    void NavigateTo<TViewModel>() where TViewModel : MaintainenaceViewModelBase;
    void GoBack();
}
