namespace EtGate.Domain
{
    // TODO: better to club these two interfaces to form a single interface: IQrDomain, because we know that all of it would be implemented by a single object
    public interface IDeviceStatus<T>
    {
        IObservable<T> statusObservable { get; }        
        bool IsWorking { get; }
    }
}
