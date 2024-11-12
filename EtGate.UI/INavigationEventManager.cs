using EtGate.UI.ViewModels;
using System;

namespace EtGate.UI;

public interface INavigationEventManager
{
    event Action<MaintainenaceViewModelBase> Navigated;
    void RaiseNavigated(MaintainenaceViewModelBase viewModel);
}
