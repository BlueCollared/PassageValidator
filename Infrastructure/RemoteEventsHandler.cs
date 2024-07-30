using Domain.Services.Modes;
using GateApp;
using Newtonsoft.Json;
using RecordsRemoteListener;
using System.IO.Pipes;

namespace Infrastructure
{
    public class MainAppServer
    {
        private readonly ModeManager modeMgr;
        private readonly IContextRepository contextRepository;

        public MainAppServer(ModeManager modeMgr, IContextRepository contextRepository)
        {
            this.modeMgr = modeMgr;
            this.contextRepository = contextRepository;
        }
        public void StartNamedPipeListener()
        {
            while (true)
            {
                using (var pipeServer = new NamedPipeServerStream("MainAppPipe", PipeDirection.InOut))
                {
                    pipeServer.WaitForConnection();

                    using (var reader = new StreamReader(pipeServer))
                    using (var writer = new StreamWriter(pipeServer) { AutoFlush = true })
                    {
                        while (true)
                        {
                            string requestJson = reader.ReadLine();
                            if (requestJson == null) break; // Client disconnected

                            BaseRequest request = JsonConvert.DeserializeObject<BaseRequest>(requestJson, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.Auto
                            });

                            BaseResponse response = ProcessRequest(request);
                            string responseJson = JsonConvert.SerializeObject(response, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.Auto
                            });

                            writer.WriteLine(responseJson);
                        }
                    }
                }
            }
        }

        private BaseResponse ProcessRequest(BaseRequest request)
        {
            switch (request)
            {
                case ChangeMode reqA:
                    return ProcessChangeMode(reqA);
                case SoftwareVersionsAtServer reqB:
                    return ProcessSoftwareVersionsAtServer(reqB);
                default:
                    throw new InvalidOperationException("Unknown request type");
            }
        }

        private ChangeModeResponse ProcessChangeMode(ChangeMode reqA)
        {
            ModeService srv = new ModeService(modeMgr, contextRepository);
            srv.ChangeMode(reqA.mode, TimeSpan.FromSeconds(20));

            throw new NotImplementedException();
            //return new ChangeModeResponse { ResultA = $"Processed MyReqA: {reqA.DataA}" };
        }

        private SoftwareVersionsAtServerResponse ProcessSoftwareVersionsAtServer(SoftwareVersionsAtServer reqB)
        {
            throw new NotImplementedException();
            //return new MyResponseB { ResultB = reqB.DataB * 2 };
        }
    }
}