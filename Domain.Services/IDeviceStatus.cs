using System.Reactive.Linq;

namespace EtGate.Domain
{
    public abstract class IDeviceStatus<T> where T:ModuleStatus
    {
        abstract public IObservable<T> statusObservable { get; }       
        
        //public bool IsWorking
        //{
        //    get
        //    {
        //        var lastStatus = statusObservable?.LastOrDefaultAsync().Wait(); // Wait for the result asynchronously
        //        return lastStatus?.IsAvailable ?? false;
        //    }
        //}
        protected IDeviceStatus()
        {
            // have to push it to the derived class which is bad
            //statusObservable.Subscribe(x => CurStatus = x);
        }

        public bool IsWorking => CurStatus != null && CurStatus.IsAvailable;
        public T CurStatus { get; protected set; }

        //public bool IsWorking => statusObservable?.LastOrDefault()?.IsAvailable ?? false;
    }
}
