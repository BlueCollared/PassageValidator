namespace EtGate.Domain
{
    public abstract class IDeviceStatus<T> where T:ModuleStatus
    {
        abstract public IObservable<T> statusObservable { get; }
        public T CurStatus { get; private set; }
        protected IDeviceStatus()
        {
            statusObservable.Subscribe(x => CurStatus = x);
        }
        public bool IsWorking =>CurStatus != null && CurStatus.IsAvailable;
    }
}
