namespace Domain.Peripherals.Qr
{
    // TODO: better to club these two interfaces to form a single interface: IQrDomain, because we know that all of it would be implemented by a single object
    public interface IQrReaderStatus
    {
        IObservable<QrReaderStatus> statusObservable { get; }
        //QrReaderStatus CurStatus { get; }
        bool IsWorking { get; }
    }

    public interface IQrInfoStatus
    {
        //string id { get; } // typically would be "Entry"/"Exit"
        IObservable<QrCodeInfo> qrCodeInfoObservable { get; }
        
        bool StartDetecting();
        void StopDetecting();
    }
}
