using System.IO.Pipes;
using System.Text.Json;

namespace NamedPipeLibrary
{
    public class Pusher
    {
        private readonly string _pipeName;
        public Pusher(string pipeName)
        {
            _pipeName = pipeName;
        }

        public void Push<T>(T message)
        {
            using (var pipeClient = new NamedPipeClientStream(".", _pipeName, PipeDirection.Out))
            {
                pipeClient.Connect();
                string serialized = JsonSerializer.Serialize(message);
                
                using (var writer = new StreamWriter(pipeClient))
                {
                    MessageWrapper wrapper = new MessageWrapper { TypeName = message.GetType().AssemblyQualifiedName, JsonPayload = serialized };
                    writer.Write(JsonSerializer.Serialize(wrapper));
                }
            }
        }
    }
}
