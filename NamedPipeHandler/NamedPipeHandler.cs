using System.IO.Pipes;
using System.Text;

namespace PipeLib
{
    public abstract class NamedPipeHandler
    {
        protected NamedPipeServerStream pipeServer;
        protected string pipeName;

        public NamedPipeHandler(string pipeName)
        {
            this.pipeName = pipeName;
            pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            StartNamedPipeServer();
        }

        private void StartNamedPipeServer()
        {
            Task.Factory.StartNew(() => ListenForClients(), TaskCreationOptions.LongRunning);
        }

        private void ListenForClients()
        {
            pipeServer.BeginWaitForConnection(ar =>
            {
                try
                {
                    pipeServer.EndWaitForConnection(ar);
                    string message = ReadFromPipe();
                    ProcessMessage(message);
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }, null);
        }

        private string ReadFromPipe()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = pipeServer.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return message;
        }

        protected void WriteToPipe(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            pipeServer.Write(buffer, 0, buffer.Length);
            pipeServer.WaitForPipeDrain();
            pipeServer.Disconnect();
            ListenForClients();
        }

        protected abstract void ProcessMessage(string message);
        protected abstract void OnError(Exception ex);
        public abstract void SendNotification<T>(T notification);
    }
}
