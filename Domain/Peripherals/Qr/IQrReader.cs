namespace Domain.Peripherals.Qr
{
    public interface IQrReader : IPeripheral, IObservable<QrReaderStatus>
    {
        string id { get; } // typically would be "Entry"/"Exit"
        IObservable<QrCodeInfo> qrCodeInfoObservable { get; }
        IObservable<QrReaderStatus> qrReaderStatusObservable { get; }
        bool StartDetecting();
        void StartListeningStatus(IObserver<QrReaderStatus> qrRdrStatus);
        void StopDetecting();
    }
}
