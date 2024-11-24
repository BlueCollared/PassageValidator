using Avalonia.Controls;
using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Qr;
using EtGate.UI;
using EtGate.UI.ViewModels.Maintenance;
using EtGate.UI.Views.Maintenance;
using Moq;

namespace MaintenanceMenu
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var view = new MaintenanceMenuView();
            var evtBus = new DeviceStatusBus<QrReaderStatus>();
            var evtStaticData = new EventBus<QrReaderStaticData>();
            var evtQrCodeInfo = new EventBus<QrCodeInfo>();
            var qrRdr = new DummyQrReaderDeviceController.DummyQrReaderDeviceController(evtBus, evtStaticData
                );//new QrReaderDeviceControllerProxy();
            var vm = new MaintenanceMenuViewModel(new Mock<INavigationService>().Object,
                new EtGate.Domain.Services.Qr.QrReaderMgr(qrRdr, evtBus, evtQrCodeInfo));

            view.DataContext = vm;
            host.Content = view;
        }
    }
}