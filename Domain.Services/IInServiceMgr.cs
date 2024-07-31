
using static Domain.Services.InService.InServiceMgr;

namespace Domain.Services.InService
{
    public interface IInServiceMgr
    {
        IObservable<State> StateObservable { get; }
        void Dispose();
        Task HaltFurtherValidations();
    }
}