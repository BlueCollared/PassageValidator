using Domain.Peripherals.Qr;
using EtGate.QrReader.Proxy;
using NamedPipeLibrary;

namespace QrReaderGuiSimulator
{
    public partial class QrReaderGuiSimulator : Form
    {
        //private ReqHandler pipeHandler;
        Pusher pusher;
        private readonly ViewModel vm = new();
        public QrReaderGuiSimulator()
        {
            InitializeComponent();
            pusher = new Pusher(QrReaderDeviceControllerProxy.pipeName);
            //pipeHandler = new ReqHandler(QrReaderDeviceControllerProxy.pipeName, vm);
        }

        private void btnStatusChanged_Click(object sender, EventArgs e)
        {
            vm.QrReaderStatus = new QrReaderStatus(chkConnected.Checked, txtFirmware.Text, chkScanning.Checked);
            //pipeHandler.SendNotification(vm.QrReaderStatus);
            pusher.Push(vm.QrReaderStatus);
        }

        private void chkStartAnswer_CheckedChanged(object sender, EventArgs e)
        {
            vm.StartResp = new StartResp();
            vm.StartResp.x = chkStartAnswer.Checked;
        }
    }
}
