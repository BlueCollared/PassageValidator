using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain.Events;
using EtGate.Domain.Services.Modes;
using System.Reactive.Linq;

namespace EtGate.Domain.Services;
public class SessionManager : ISessionManager
{
    private readonly IMessagePublisher<AgentLoggedIn> evtAgentLoggedIn;
    private readonly IMessagePublisher<ShiftTerminated> evtShiftTerminated;

    private readonly IMessagePublisher<ShiftTimedOut> evtShiftTimedOut;
    OneShotTimer2 timer;
    public SessionManager(
        IMessagePublisher<AgentLoggedIn> evtAgentLoggedIn,
        IMessagePublisher<ShiftTerminated> evtShiftTerminated,
        IMessagePublisher<ShiftTimedOut> evtShiftTimedOut)
    {
        this.evtAgentLoggedIn = evtAgentLoggedIn;
        this.evtShiftTerminated = evtShiftTerminated;
        this.evtShiftTimedOut = evtShiftTimedOut;        
    }

    public AgentLoggedInResult AgentLoggedIn()
    {
        timer?.Stop();
        timer?.Dispose();
        timer = null;

        timer = new OneShotTimer2();
        timer.Ticks
        .SubscribeOn(SynchronizationContext.Current)
        .Subscribe(_ =>
        {
            evtShiftTimedOut.Publish(new ShiftTimedOut());
        });
        timer.SetInterval(TimeSpan.FromSeconds(10));
        return new AgentLoggedInResult();
    }

    public void AgentLoggedOut()
    {
        timer?.Stop();
        timer?.Dispose();
        timer = null;

        evtShiftTerminated.Publish(new ShiftTerminated());
    }
}
