using EtGate.Domain;
using GateApp;

namespace EtGate.DependencyInjection;

public class MockContextRepository : IContextRepository
{
    public void SaveMode(OpMode mode)
    {
        throw new NotImplementedException();
    }
}