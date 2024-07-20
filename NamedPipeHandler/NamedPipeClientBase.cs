using Microsoft.VisualBasic;
using NamedPipeHandler;
using Newtonsoft.Json;
using System.IO.Pipes;
using System.Reflection;
using System.Text;

public abstract class NamedPipeClientBase
{
    private readonly string pipeName;
    private readonly List<Type> notificationTypes;

    protected NamedPipeClientBase(string pipeName, List<Type> notificationTypes, List<Type> requestTypes)
    {
        this.pipeName = pipeName;
        this.notificationTypes = notificationTypes;

        StartListeningForNotifications(CancellationToken.None);

        //Dictionary<string, Type> typeMappings = new();
        //foreach (var reqType in requestTypes)
        //    typeMappings[reqType.Name] = reqType;
        
        //settings.Converters.Add(new RequestConverter(typeMappings));
        //settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    JsonSerializerSettings settings = new();

    public async Task<TResponse> RequestReplyAsync<TRequest, TResponse>(TRequest request, TimeSpan timeout) 
        where TRequest:IRequest
        where TResponse:IResponse
    {
        using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous))
        {
            await client.ConnectAsync();

            var message = JsonConvert.SerializeObject(request, settings);
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await client.WriteAsync(buffer, 0, buffer.Length);

            var cts = new CancellationTokenSource(timeout);
            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    buffer = new byte[1024];
                    int bytesRead = await client.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    var deserializedResponse = JsonConvert.DeserializeObject<TResponse>(response);
                    if (deserializedResponse != null)
                    {
                        return deserializedResponse;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException("The request timed out.");
            }

            throw new InvalidOperationException("No valid response received.");
        }
    }
    //public async Task SendRequestAsync<TRequest>(TRequest request)
    //{
    //    using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous))
    //    {
    //        await client.ConnectAsync();

    //        var message = JsonConvert.SerializeObject(request);
    //        byte[] buffer = Encoding.UTF8.GetBytes(message);
    //        await client.WriteAsync(buffer, 0, buffer.Length);

    //        buffer = new byte[1024];
    //        int bytesRead = await client.ReadAsync(buffer, 0, buffer.Length);
    //        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

    //        ProcessResponse(response);
    //    }
    //}

    public void StartListeningForNotifications(CancellationToken cancellationToken)
    {
        Task.Run(async () => await ListenForNotifications(cancellationToken), cancellationToken);
    }

    private async Task ListenForNotifications(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.In))
                {
                    await client.ConnectAsync();

                    byte[] buffer = new byte[1024];
                    int bytesRead = await client.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    foreach (var notificationType in notificationTypes)
                    {
                        var notification = JsonConvert.DeserializeObject(message, notificationType);
                        if (notification != null)
                        {
                            var processNotificationMethod = GetType().GetMethod(nameof(ProcessNotification), BindingFlags.NonPublic | BindingFlags.Instance);
                            var genericMethod = processNotificationMethod.MakeGenericMethod(notificationType);
                            genericMethod.Invoke(this, new[] { notification });

                            break;
                        }
                    }                    
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }
    }

    //protected abstract void ProcessResponse(string response);
    protected abstract void ProcessNotification(NotificationMessageBase notification);
    protected abstract void OnError(Exception ex);
}

public abstract class NotificationMessageBase
{
    public string Type { get; set; }
}
