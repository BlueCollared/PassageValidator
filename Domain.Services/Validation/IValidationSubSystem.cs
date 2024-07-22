using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services.Validation
{
    public interface IValidationSubSystem : IValidate
    {
        IObservable<OfflineValidationSystemStatus> statusObservable { get; }
        bool IsWorking { get; }
    }
}