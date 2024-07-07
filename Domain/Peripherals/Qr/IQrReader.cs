namespace Domain.Peripherals.Qr
{
    public interface IQrReader : IPeripheral, IObservable<QrCodeInfo>, IObservable<QrReaderStatus>
    {
        string id { get; } // typically would be "Entry"/"Exit"
        bool StartDetecting();
        void StartListeningStatus(IObserver<QrReaderStatus> qrRdrStatus);
        void StopDetecting();
    }
}
