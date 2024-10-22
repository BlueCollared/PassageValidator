using System.Reactive.Linq;

namespace EtGate.Domain
{
    public abstract class IDeviceStatus<T> where T:ModuleStatus
    {
        abstract public IObservable<T> statusObservable { get; }       
        
        public bool IsWorking
        {
            get
            {
                var lastStatus = statusObservable?.LastOrDefaultAsync().Wait(); // Wait for the result asynchronously
                return lastStatus?.IsAvailable ?? false;
            }
        }
        //public bool IsWorking => statusObservable?.LastOrDefault()?.IsAvailable ?? false;
    }
}
