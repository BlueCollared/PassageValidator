using Avalonia.Controls;
using EtGate.UI.ViewModels.Maintenance;
using EtGate.UI.Views.Maintenance;

namespace MaintenanceMenu
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var view = new MaintenanceMenuView();

            var qrRdr = new DummyQrReaderDeviceController.DummyQrReaderDeviceController();//new QrReaderDeviceControllerProxy();
            var vm = new MaintenanceMenuViewModel(new EtGate.Domain.Services.Qr.QrReaderMgr(qrRdr, qrRdr));

            view.DataContext = vm;
            host.Content = view;
        }
    }
}