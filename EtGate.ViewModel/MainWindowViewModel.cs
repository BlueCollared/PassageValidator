using Domain.Services.InService;
using Equipment.Core.Message;
using EtGate.Domain;
using ReactiveUI;
using System.Reactive.Linq;

namespace EtGate.UI.ViewModels;

// `bEntry` and `bPrimary` are smells. But to avoid them, means making 4 new classes. I don't know better way.
public class MainWindowViewModel : ViewModelBase
{
    
    private readonly IModeViewModelFactory modeViewModelFactory;
    private readonly bool bEntry;
    private readonly bool bPrimary;

    public MainWindowViewModel(IModeViewModelFactory modeViewModelFactory,
        DeviceStatusSubscriber<(Mode, ISubModeMgr)> modeService,
        bool bEntry,
        bool bPrimary)
    {        
        this.modeViewModelFactory = modeViewModelFactory;
        this.bEntry = bEntry;
        this.bPrimary = bPrimary;
        modeService.Messages.Subscribe(x => ModeChanged(x.Item1, x.Item2));
    }

    private void ModeChanged(Mode mode, ISubModeMgr subModeMgr)
    {
        if (curMode == mode)
            return;
        if (CurrentModeViewModel is IDisposable y)
            y.Dispose();
        CurrentModeViewModel = modeViewModelFactory.Create(mode, subModeMgr, bPrimary, bEntry);
        if (CurrentModeViewModel is MaintenanceViewModel r)
            r.Init();
        curMode = mode;
    }

    private ModeViewModel _currentModeViewModel;
    public ModeViewModel CurrentModeViewModel
    {
        get => _currentModeViewModel;
        private set
        {
            if (_currentModeViewModel != value)
            {
                _currentModeViewModel = value;
                this.RaisePropertyChanged(nameof(CurrentModeViewModel));
            }
        }
    }

    private Mode? curMode = null;    
}
