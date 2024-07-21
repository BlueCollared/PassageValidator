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
        private readonly Delegate _userTypeHandler;
        private readonly Type _userType;

        public Puller(string pipeName, HandleNotification notificationHandler, 
            List<Type> notificationTypes,
            Delegate userTypeHandler, Type userType)
        {
            _pipeName = pipeName;
            _notificationHandler = notificationHandler;
            this.notificationTypes = notificationTypes;
            _userTypeHandler = userTypeHandler;
            _userType = userType;
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
                        var json = await reader.ReadToEndAsync();
                        foreach(var typ in notificationTypes)
                        {
                            object objx = JsonSerializer.Deserialize(json, typ);
                            if (objx != null)
                            {
                                _notificationHandler(objx);
                                break;
                            }
                        }
                        var obj = JsonSerializer.Deserialize(json, typeof(object));

                        //if (obj is INotificationReceived notification)
                        //{
                        //    _notificationHandler(notification);
                        //}
                        if (obj != null && _userType.IsInstanceOfType(obj))
                        {
                            var method = _userTypeHandler.GetMethodInfo();
                            if (method != null)
                            {
                                method.Invoke(_userTypeHandler.Target, new[] { obj });
                            }
                        }
                    }
                }
            }
        }
    }    
}
