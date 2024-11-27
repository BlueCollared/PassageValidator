using Domain.Services.Modes;
using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Qr;
using GateApp;
using ReactiveUI;
using System.Reactive.Linq;

namespace EtGate.UI.ViewModels
{
    public class AppBootingViewModel : ModeViewModel, IDisposable
    {
        public AppBootingViewModel(IModeManager modeService,
            DeviceStatusSubscriber<QrReaderStatus> qr) : base(modeService)
        {
            //_qrRdrStatus = qr.Messages.ToProperty(this, x => x.QrRdrStatus, scheduler: RxApp.MainThreadScheduler);
            qr?.Messages.ObserveOn(RxApp.MainThreadScheduler)
           .Subscribe(status => QrRdrStatus = status.ToString());
        }

        private string _qrRdrStatus;

        public string QrRdrStatus
        {
            get => _qrRdrStatus;
            set => this.RaiseAndSetIfChanged(ref _qrRdrStatus, value);
        }
        //private readonly ObservableAsPropertyHelper<string> _qrRdrStatus;

        // Read-only property for QrRdrStatus
        //public string QrRdrStatus => _qrRdrStatus.Value;
        public void Dispose()
        {
            
        }
    }
}
