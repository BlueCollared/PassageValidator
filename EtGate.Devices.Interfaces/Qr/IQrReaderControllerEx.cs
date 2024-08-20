using Peripherals;

namespace EtGate.Devices.Interfaces.Qr
{
    interface IQrReaderControllerEx : IPeripheral, Domain.Services.Qr.IQrReaderController
    {
        void Reboot();
    }
}