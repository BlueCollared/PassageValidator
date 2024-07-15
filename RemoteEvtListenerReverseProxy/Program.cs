using Newtonsoft.Json;
using RecordsRemoteListener;
using System.IO.Pipes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapPost("/setMode", (int mode) =>
{
    ChangeMode cm = new();
    // TODO: send acknowledgement immediatly. See how can it be achieved

    BaseResponse resp = SendToMainAppAndGetResponse(cm);
    return (ChangeModeResponse)resp;
});
app.Run();

BaseResponse SendToMainAppAndGetResponse(BaseRequest request)
{
    string requestJson = JsonConvert.SerializeObject(request, new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto
    });

    using (var pipeClient = new NamedPipeClientStream(".", "MainAppPipe", PipeDirection.InOut))
    {
        pipeClient.Connect();

        using (var reader = new StreamReader(pipeClient))
        using (var writer = new StreamWriter(pipeClient) { AutoFlush = true })
        {
            writer.WriteLine(requestJson);
            string responseJson = reader.ReadLine();
            return JsonConvert.DeserializeObject<BaseResponse>(responseJson, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}