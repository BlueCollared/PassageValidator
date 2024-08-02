using Domain.Peripherals.Qr;
using EtGate.Domain.Services.Qr;
using ReactiveUI;
using System;
using System.Reactive.Linq;


namespace EtGate.UI.ViewModels.Maintenance
{
    public class MaintenanceMenuViewModel : ViewModelBase
    {
        private readonly QrReaderMgr qrReaderMgr;

        public bool bQrWorking { get; set; }

        public MaintenanceMenuViewModel(QrReaderMgr qrReaderMgr)
        {
            this.qrReaderMgr = qrReaderMgr;
            qrReaderMgr.StatusStream.Subscribe(onNext:
                x => { QrRdrStatusChanged(x); });
        }

        private void QrRdrStatusChanged(QrReaderStatus x)
        {
            bQrWorking = x.IsAvailable;
            this.RaisePropertyChanged(nameof(bQrWorking));
        }
    }
}
