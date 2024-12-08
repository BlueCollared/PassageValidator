namespace EtGate.Domain.Services;

public interface ISessionManager {
    AgentLoggedInResult AgentLoggedIn();
    void AgentLoggedOut();
}
