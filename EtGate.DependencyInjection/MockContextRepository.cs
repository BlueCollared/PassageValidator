using EtGate.Domain;
using GateApp;
using System;

namespace EtGate.UI;

public class MockContextRepository : IContextRepository
{
    public void SaveMode(OpMode mode)
    {
        throw new NotImplementedException();
    }
}