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
            {
                parent.Notify(x);
            }
        }

        static void HandleViewModelChange(ViewModel message)
        {
            //Console.WriteLine($"Received custom message: {message.Content}");
        }

    }
}
