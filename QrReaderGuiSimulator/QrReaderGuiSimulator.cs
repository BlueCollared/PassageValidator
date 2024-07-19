namespace QrReaderGuiSimulator
{
    public partial class QrReaderGuiSimulator : Form
    {
        private ReqHandler pipeHandler;
        private readonly ViewModel vm = new();
        public QrReaderGuiSimulator()
        {
            InitializeComponent();
            pipeHandler = new ReqHandler("QrSimul", vm);
        }
    }
}
