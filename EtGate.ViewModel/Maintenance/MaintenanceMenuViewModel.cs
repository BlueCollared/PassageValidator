using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.Services.Qr;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows.Input;


namespace EtGate.UI.ViewModels.Maintenance
{
    public class MaintenanceMenuViewModel : MaintainenaceViewModelBase
    {        
        private readonly IQrReaderMgr qrReaderMgr;

        public bool bQrWorking { get; set; }

        public MaintenanceMenuViewModel(INavigationService navService, IQrReaderMgr qrReaderMgr, DeviceStatusSubscriber<QrReaderStatus> entryQr) : base(navService)
        {            
            this.qrReaderMgr = qrReaderMgr;
            entryQr.Messages.Subscribe(onNext:
                x => { QrRdrStatusChanged(x); });

            FlapMaintenanceSelectedCommand = ReactiveCommand.Create(()=>navService.NavigateTo<FlapMaintenanceViewModel>());
        }

        private void QrRdrStatusChanged(QrReaderStatus x)
        {
            bQrWorking = x.IsAvailable;
            this.RaisePropertyChanged(nameof(bQrWorking));
        }

        public override void Dispose()
        {
            IsDisposed = true;
        }

        public ICommand FlapMaintenanceSelectedCommand { get; private set; }        
    }
}
