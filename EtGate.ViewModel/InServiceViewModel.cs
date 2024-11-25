using Domain.Services.InService;
using GateApp;
using ReactiveUI;
using System.Reactive.Linq;
using static Domain.Services.InService.InServiceMgr;

namespace EtGate.UI.ViewModels
{
    public class InServiceViewModel : ModeViewModel, IDisposable
    {
        //private readonly IInServiceMgr _inServiceMgr;
        
        private readonly bool _isEntry;

        //private readonly ObservableAsPropertyHelper<string> _notificationMessage;
        //private string _statusMessage;

        //public string StatusMessage
        //{
        //    get => _statusMessage;
        //    private set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
        //}
        CompoundState state;
        
        public CompoundState CurState
        {
            get => state;
            set => this.RaiseAndSetIfChanged(ref state, value);
        }

        IDisposable subscription;

        public InServiceViewModel(            
            bool isEntry, IModeCommandService modeService, IInServiceMgr inServiceMgr) : base(modeService)
        {            
            _isEntry = isEntry;

            if (_isEntry)
                subscription = inServiceMgr.StateObservable.Select(x => ConvertForEntry(x))
                    .Subscribe(x => { CurState = x; });
            else
                subscription = inServiceMgr.StateObservable.Select(x => ConvertForExit(x))
                    .Subscribe(x => { CurState = x; });
            //_currentState = ((InServiceMgr)(modeService.curModeMgr)).StateObservable.ToProperty(this, x => x.CurrentState);
            //_notificationMessage = this.WhenAnyValue(x => x.CurrentState)
            //                      .Select(state => state.ToString())
            //                      .ToProperty(this, x => x.NotificationMessage);
        }

        private static CompoundState ConvertForEntry(State state)
        {
            switch(state)
            {
                case State.Idle:
                    return CompoundState.Idle;
                case State.ValidationAtEntryInProgress:
                    return CompoundState.ValidationInProgress;
                case State.PassageAuthroizedAtEntry:
                    return CompoundState.PassageAuthroized;
                case State.ValidationAtExitInProgress:
                case State.PassageAuthroizedAtExit:
                    return CompoundState.ProhibitedTemp;                
            }
            throw new NotImplementedException();            
        }

        private static CompoundState ConvertForExit(State state)
        {
            switch (state)
            {
                case State.Idle:
                    return CompoundState.Idle;
                case State.ValidationAtEntryInProgress:
                case State.PassageAuthroizedAtEntry:
                    return CompoundState.ProhibitedTemp;                
                case State.ValidationAtExitInProgress:
                    return CompoundState.ValidationInProgress;
                case State.PassageAuthroizedAtExit:
                    return CompoundState.PassageAuthroized;
            }
            throw new NotImplementedException();
        }


        //private readonly ObservableAsPropertyHelper<InServiceMgr.State> _currentState;
        //public string NotificationMessage => _notificationMessage.Value;

        //public InServiceMgr.State CurrentState => _currentState.Value;

        bool bDisposed = false;
        public void Dispose()
        {
            if (!bDisposed)
            {
                bDisposed = true;
                subscription.Dispose();
            }
        }
    }
}
