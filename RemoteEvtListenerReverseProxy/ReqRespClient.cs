using Newtonsoft.Json;
using RecordsRemoteListener;
using System.IO.Pipes;

namespace RemoteEvtListenerReverseProxy
{
    public class ScadaClient
    {
        private NamedPipeClientStream pipeClient;
        private StreamReader reader;
        private StreamWriter writer;

        public ScadaClient()
        {
            Connect();
        }

        private void Connect()
        {
            pipeClient = new NamedPipeClientStream(".", "MainAppPipe", PipeDirection.InOut);
            pipeClient.Connect();

            reader = new StreamReader(pipeClient);
            writer = new StreamWriter(pipeClient) { AutoFlush = true };
        }

        public BaseResponse SendToMainAppAndGetResponse(BaseRequest request)
        {
            string requestJson = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            writer.WriteLine(requestJson);
            string responseJson = reader.ReadLine();
            return JsonConvert.DeserializeObject<BaseResponse>(responseJson, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public void Disconnect()
        {
            writer.Close();
            reader.Close();
            pipeClient.Close();
        }
    }
}
