
namespace EtGate.Domain.Services
{
    public interface ILoginService
    {
        Task<Agent?> Login(string userId, string passwd);
        void Logout();
    }
}