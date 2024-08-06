using static Domain.Services.InService.InServiceMgr;

namespace Domain.Services.InService
{
    public interface IInServiceMgr : IDisposable
    {
        IObservable<State> StateObservable { get; }        
        Task HaltFurtherValidations();
    }
}