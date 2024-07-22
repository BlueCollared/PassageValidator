using EtGate.Domain.ValidationSystem;

namespace EtGate.Domain.Services.Validation
{
    public interface IValidationSubSystem : IValidate
    {
        IObservable<ValidataionSubSystemStatus> statusObservable { get; }        
    }
}