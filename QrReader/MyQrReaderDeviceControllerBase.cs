using EtGate.Devices.Interfaces.Qr;

namespace EtGate.QrReader
{
    // I'm absolutly confused whether this base class is needed or not. Other way would have been:
    // QrReaderDeviceController: QrReaderDeviceControllerBase, IQrReaderControllerDeviceSpecific
    // So, might have to adopt the above instead.
    public abstract class MyQrReaderDeviceControllerBase : QrReaderDeviceControllerBase, IQrReaderControllerDeviceSpecific
    {
        public abstract void Reboot();
    }
}
