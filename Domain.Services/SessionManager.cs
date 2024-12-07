using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain.Events;
using System.Reactive.Linq;

namespace EtGate.Domain.Services;

public class SessionManager
{
    private readonly IMessageSubscriber<AgentLoggedIn> evtAgentLoggedIn;
    
    public SessionManager(IMessageSubscriber<AgentLoggedIn> evtAgentLoggedIn)
    {
        MyTimer timer;
        this.evtAgentLoggedIn = evtAgentLoggedIn;
        this.evtAgentLoggedIn.Messages.Subscribe(x => {
            //Observable.Timer(TimeSpan.FromSeconds(10)).Subscribe(
        });
    }
}
