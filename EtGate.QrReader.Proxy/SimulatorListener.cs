using Domain.Peripherals.Qr;

namespace EtGate.QrReader.Proxy
{
    internal class SimulatorListener
    {
        static QrReaderDeviceControllerProxy parent;
        public SimulatorListener(string pipeName, QrReaderDeviceControllerProxy parent)
        {
            SimulatorListener.parent = parent;
            var puller = new NamedPipeLibrary.Puller(
                pipeName,                
                HandleNotification,
                new List<Type> { typeof(QrReaderStatus), typeof(QrCodeInfo), typeof(ViewModel)}
                //(NamedPipeLibrary.Puller.HandleUserSuppliedType<ViewModel>)HandleViewModelChange,
                //typeof(ViewModel)
                );            
        }

        static void HandleNotification(object notification)
        {
            if (notification is QrReaderStatus x)
                parent.Notify(x);
            else if (notification is QrCodeInfo y)
                parent.Notify(y);
            else if (notification is ViewModel vm)
                HandleViewModelChange(vm);
        }

        static void HandleViewModelChange(ViewModel message)
        {
            parent.StartAnswer = message.StartResp.x;
            parent.StartDetectingAnswer = message.StartDetectingResp.x;
        }
    }
}
