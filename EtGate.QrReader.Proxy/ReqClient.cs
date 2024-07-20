

namespace EtGate.QrReader.Proxy
{
    internal class ReqClient : NamedPipeClientBase
    {
        public ReqClient(string pipeName, List<Type> notificationTypes, List<Type> requestTypes) : base(pipeName, notificationTypes, requestTypes)
        {
        }

        protected override void OnError(Exception ex)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessNotification(NotificationMessageBase notification)
        {
            
        }
    }
}
