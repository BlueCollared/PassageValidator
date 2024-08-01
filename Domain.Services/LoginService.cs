namespace EtGate.Domain.Services
{    
    public class LoginService
    {
        public async Task<Agent?> Login(string userId, string passwd)
        {
            Agent agent = new();
            agent.id = "Agent_X";
            agent.name = "Frank";
            agent.opTyp = OpTyp.Maintenace;
            
            return agent;
        }

        public void Logout()
        { }
    }
}
