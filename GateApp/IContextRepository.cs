using EtGate.Domain;

namespace GateApp
{
    public interface IContextRepository
    {
        void SaveMode(OpMode mode);
    }
}