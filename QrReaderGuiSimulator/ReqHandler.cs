using EtGate.QrReader.Proxy;
using Newtonsoft.Json;
using System.IO.Pipes;
using System.Text;

namespace QrReaderGuiSimulator
{
    internal class ReqHandler : NamedPipeHandler
    {
        private readonly ViewModel viewModel;

        public ReqHandler(string pipeName, ViewModel viewModel)
            : base(pipeName)
        {
            this.viewModel = viewModel;
        }

        protected override void ProcessMessage(string message)
        {
            dynamic json = JsonConvert.DeserializeObject(message);

            if (json.StartDetectingReq != null)
            {
                StartDetectingReq req = JsonConvert.DeserializeObject<StartDetectingReq>(message);
                string response = JsonConvert.SerializeObject(viewModel.StartDetectingResp);
                WriteToPipe(response);
            }
            else if (json.Start != null)
            {
                StartReq req = JsonConvert.DeserializeObject<StartReq>(message);
                string response = JsonConvert.SerializeObject(viewModel.StartResp);
                WriteToPipe(response);
            }
        }

        protected override void OnError(Exception ex)
        {
            // Handle error (e.g., log or show a message box)
        }

        public override void SendNotification(string notification)
        {
            using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
            {
                client.Connect();
                byte[] buffer = Encoding.UTF8.GetBytes(notification);
                client.Write(buffer, 0, buffer.Length);
            }
        }
    }
}