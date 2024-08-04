
using static Domain.Services.InService.InServiceMgr;

namespace Domain.Services.InService
{
    public interface IInServiceMgr
    {
        IObservable<State> StateObservable { get; }
        void Dispose(); // TODO: why this interface is not implementing IDisposable
        Task HaltFurtherValidations();
    }
}