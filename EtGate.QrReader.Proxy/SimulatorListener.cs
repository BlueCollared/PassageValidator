using Domain.Peripherals.Qr;
using QrReaderGuiSimulator;

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
                new List<Type> { typeof(QrReaderStatus)},
                (NamedPipeLibrary.Puller.HandleUserSuppliedType<ViewModel>)HandleViewModelChange,
                typeof(ViewModel)
                );            
        }

        static void HandleNotification(object notification)
        {
            if (notification is QrReaderStatus x)            
                parent.Notify(x);
            else if (notification is QrCodeInfo y)
                parent.Notify(y);
        }

        static void HandleViewModelChange(ViewModel message)
        {
            parent.StartAnswer = message.StartResp.x;
            parent.StartDetectingAnswer = message.StartDetectingResp.x;
        }
    }
}
