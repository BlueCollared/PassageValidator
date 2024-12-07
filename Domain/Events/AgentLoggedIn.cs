namespace EtGate.Domain.Events;

public class AgentLoggedIn
{
    public int agentId {  get; set; }
    public DateTimeOffset loginTime { get; set; }
}