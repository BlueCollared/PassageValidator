using EtGate.Domain.Peripherals.Qr;
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
            vm.StartDetectingResp = new StartDetectingResp() { x = chkStartDetectAnswer.Checked };
            vm.StartResp = new StartResp() { x = chkStartAnswer.Checked };
            //pipeHandler = new ReqHandler(QrReaderDeviceControllerProxy.pipeName, vm);
        }

        private void btnStatusChanged_Click(object sender, EventArgs e)
        {
            vm.QrReaderStatus = new QrReaderStatus(chkConnected.Checked, chkScanning.Checked);
            //pipeHandler.SendNotification(vm.QrReaderStatus);
            pusher.Push(vm.QrReaderStatus);
        }

        private void chkStartAnswer_CheckedChanged(object sender, EventArgs e)
        {
            vm.StartResp = new StartResp();
            vm.StartResp.x = chkStartAnswer.Checked;
            PushNewVM();
        }

        private void PushNewVM()
        {
            pusher.Push(vm);
        }

        private void chkStartDetectAnswer_CheckedChanged(object sender, EventArgs e)
        {
            vm.StartDetectingResp = new StartDetectingResp() { x = chkStartDetectAnswer.Checked };
            PushNewVM();
        }

        private void btnSynchronize_Click(object sender, EventArgs e)
        {
            PushNewVM();
        }
    }
}