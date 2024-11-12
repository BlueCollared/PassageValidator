using EtGate.UI.ViewModels;
using System;

namespace EtGate.UI;

public class NavigationEventManager : INavigationEventManager
{
    public event Action<MaintainenaceViewModelBase> Navigated;

    public void RaiseNavigated(MaintainenaceViewModelBase viewModel)
    {
        Navigated?.Invoke(viewModel);
    }
}
