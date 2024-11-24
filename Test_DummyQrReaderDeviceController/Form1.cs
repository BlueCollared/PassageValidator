using Equipment.Core;
using EtGate.Domain.Peripherals.Qr;
using System.Reactive.Linq;

namespace Test_DummyQrReaderDeviceController
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            qrController = new DummyQrReaderDeviceController.DummyQrReaderDeviceController(qrStatus, null);
            InitializeComponent();
            qrStatus.Messages
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x =>
                {
                    txtLog.Text += x.bConnected.ToString();
                });
        }

        DeviceStatusBus<QrReaderStatus> qrStatus = new();
        //DeviceStatusBus<QrReaderStaticData> qrStaticData = new();
        DummyQrReaderDeviceController.DummyQrReaderDeviceController qrController;

        CancellationTokenSource cts;
        private async void btnStartDetection_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            await qrController.StartDetecting(cts.Token);
        }

        private void btnStopDetect_Click(object sender, EventArgs e)
        {
            if (cts.Token.IsCancellationRequested)
                return;
            cts.Cancel();
        }
    }
}
