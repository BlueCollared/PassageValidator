using System.IO.Pipes;
using System.Reflection;
using System.Text.Json;

namespace NamedPipeLibrary
{
    public class Puller
    {
        private readonly string _pipeName;
        public delegate void HandleNotification(object notification);
        public delegate void HandleUserSuppliedType<T>(T obj);

        private readonly HandleNotification _notificationHandler;
        private readonly List<Type> notificationTypes;
        //private readonly Delegate _userTypeHandler;
        //private readonly Type _userType;

        public Puller(string pipeName, HandleNotification notificationHandler, 
            List<Type> notificationTypes
            //Delegate userTypeHandler, 
            //Type userType
            )
        {
            _pipeName = pipeName;
            _notificationHandler = notificationHandler;
            this.notificationTypes = notificationTypes;
          //  _userTypeHandler = userTypeHandler;
            //_userType = userType;
            Task.Factory.StartNew(async () => { await StartListeningAsync(); });
        }
        public async Task StartListeningAsync()
        {
            while (true)
            {
                using (var pipeServer = new NamedPipeServerStream(_pipeName, PipeDirection.In))
                {
                    await pipeServer.WaitForConnectionAsync();

                    using (var reader = new StreamReader(pipeServer))
                    {
                        var msgWhole = await reader.ReadToEndAsync();
                        var wrapper = JsonSerializer.Deserialize<MessageWrapper>(msgWhole);

                        var type = Type.GetType(wrapper.TypeName);
                        var message = JsonSerializer.Deserialize(wrapper.JsonPayload, type);

                        foreach (var typ in notificationTypes)
                        {
                            if (typ.AssemblyQualifiedName == type.AssemblyQualifiedName)
                            {
                                _notificationHandler(message);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }    
}