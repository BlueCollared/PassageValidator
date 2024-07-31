using Domain.Services.InService;
using EtGate.UI.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;

namespace EtGate.ViewModel
{
    public class InServiceViewModel : ViewModelBase
    {
        private readonly IInServiceMgr _inServiceMgr;
        private readonly ObservableAsPropertyHelper<string> _notificationMessage;
        private readonly bool _isEntry;
        private string _statusMessage;

        public string StatusMessage
        {
            get => _statusMessage;
            private set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
        }

        public InServiceViewModel(IInServiceMgr inServiceMgr, bool isEntry)
        {
            _inServiceMgr = inServiceMgr;
            _isEntry = isEntry;
            //    _inServiceMgr.StateChanged += OnStateChanged;

            _currentState = _inServiceMgr.StateObservable.ToProperty(this, x => x.CurrentState);
            _notificationMessage = this.WhenAnyValue(x => x.CurrentState)
                                  .Select(state => state.ToString())
                                  .ToProperty(this, x => x.NotificationMessage);
        }

        private readonly ObservableAsPropertyHelper<InServiceMgr.State> _currentState;
        public string NotificationMessage => _notificationMessage.Value;

        public InServiceMgr.State CurrentState => _currentState.Value;
    }
}
