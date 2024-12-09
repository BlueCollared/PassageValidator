using Domain.Services.Modes;
using Equipment.Core.Message;
using EtGate.Domain.Events;
using EtGate.Domain.Services.Modes;
using EtGate.UI.ViewModels.Maintenance;
using ReactiveUI;
using System.Reactive.Linq;

namespace EtGate.UI.ViewModels;

public class MaintenanceViewModel : ModeViewModel
{
    private readonly INavigationService _navigationService;
    public Interaction<string, bool> ShowShiftTimedOutDialog { get; }

    public MaintenanceViewModel(IModeManager modeService, INavigationService navigationService,
        IMessageSubscriber<ShiftTimedOut> shiftTimedOut,
        IMessagePublisher<ShiftTerminated> evtShiftTerminated) 
        : base(modeService)
    {
        ShowShiftTimedOutDialog = new Interaction<string, bool>();

        _navigationService = navigationService;
        shiftTimedOut.Messages
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(async x=> {
            bool result = await ShowShiftTimedOutDialog.Handle("Shift timed out. Do you want to continue?");
            if (result)
            {
                    // Handle the "Yes" response
                    evtShiftTerminated.Publish(new ShiftTerminated());
                }
            else
            {
                // Handle the "No" response
            }
        });
    }

    public void Init()
    {
        _navigationService.NavigateTo<AgentLoginViewModel>();
    }
}