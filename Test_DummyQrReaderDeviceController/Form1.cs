
using Domain.Peripherals.Qr;
using Equipment.Core;
using System.Reactive.Linq;

namespace Test_DummyQrReaderDeviceController
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            qrController = new DummyQrReaderDeviceController.DummyQrReaderDeviceController(qrStatus, null);
            InitializeComponent();
        }
        DeviceStatusBus<QrReaderStatus> qrStatus = new();
        //DeviceStatusBus<QrReaderStaticData> qrStaticData = new();
        DummyQrReaderDeviceController.DummyQrReaderDeviceController qrController;

        private void btnStartDetection_Click(object sender, EventArgs e)
        {
            qrController.StartDetecting();
            qrStatus.Messages
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x =>
            {
                txtLog.Text += x.bConnected.ToString();
            });
        }

        private void btnStopDetect_Click(object sender, EventArgs e)
        {
            qrController.StopDetecting();
        }
    }
}
