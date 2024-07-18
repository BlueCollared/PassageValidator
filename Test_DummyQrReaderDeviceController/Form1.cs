
namespace Test_DummyQrReaderDeviceController
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DummyQrReaderDeviceController.DummyQrReaderDeviceController qrController = new();

        private void btnStartDetection_Click(object sender, EventArgs e)
        {
            qrController.StartDetecting();
            qrController.qrReaderStatusObservable.Subscribe(x =>
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
