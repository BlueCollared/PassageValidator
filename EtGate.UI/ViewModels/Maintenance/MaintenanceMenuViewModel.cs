using Domain.Peripherals.Qr;
using EtGate.Domain.Services.Qr;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows.Input;


namespace EtGate.UI.ViewModels.Maintenance
{
    public class MaintenanceMenuViewModel : MaintainenaceViewModelBase
    {        
        private readonly IQrReaderMgr qrReaderMgr;

        public bool bQrWorking { get; set; }

        public MaintenanceMenuViewModel(INavigationService navService, IQrReaderMgr qrReaderMgr) : base(navService)
        {            
            this.qrReaderMgr = qrReaderMgr;
            qrReaderMgr.StatusStream.Subscribe(onNext:
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
            
        }

        public ICommand FlapMaintenanceSelectedCommand { get; private set; }        
    }
}
