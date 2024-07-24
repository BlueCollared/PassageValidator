namespace EtGate.Domain
{
    // TODO: better to club these two interfaces to form a single interface: IQrDomain, because we know that all of it would be implemented by a single object
    public abstract class IDeviceStatus<T> where T:ModuleStatus
    {
        public IObservable<T> statusObservable { get; }
        public T CurStatus { get; private set; }
        protected IDeviceStatus()
        {
            statusObservable.Subscribe(x => CurStatus = x);
        }
        public bool IsWorking =>CurStatus != null && CurStatus.IsAvailable;
    }
}
