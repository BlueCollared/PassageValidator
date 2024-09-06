using Domain;
using GateApp;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace EtGate.UI.ViewModels;

// `bEntry` and `bPrimary` are smells. But to avoid them, means making 4 new classes. I don't know better way.
public class MainWindowViewModel : ViewModelBase
{
    
    private readonly IModeViewModelFactory modeViewModelFactory;
    private readonly bool bEntry;
    private readonly bool bPrimary;

    public MainWindowViewModel(IModeViewModelFactory modeViewModelFactory,
        IModeService modeService,
        bool bEntry,
        bool bPrimary)
    {        
        this.modeViewModelFactory = modeViewModelFactory;
        this.bEntry = bEntry;
        this.bPrimary = bPrimary;
        modeService.EquipmentModeObservable.Subscribe(x => ModeChanged(x));
    }

    private void ModeChanged(Mode x)
    {
        if (curMode == x)
            return;
        if (CurrentModeViewModel is IDisposable y)
            y.Dispose();
        CurrentModeViewModel = modeViewModelFactory.Create(x, bPrimary, bEntry);
        curMode = x;
    }

    private ModeViewModel _currentModeViewModel;
    public ModeViewModel CurrentModeViewModel
    {
        get => _currentModeViewModel;
        set
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
