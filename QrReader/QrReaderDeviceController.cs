using Domain.Peripherals.Qr;
using EtGate.QrReader;
using Peripherals;

namespace QrReader
{
    interface IQrReader : IPeripheral { }

    public class QrReaderDeviceController : QrReaderDeviceControllerBase
    {
        public override bool Start()
        {
            throw new NotImplementedException();
        }

        public override bool StartDetecting()
        {
            throw new NotImplementedException();
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }

        public override void StopDetecting()
        {
            throw new NotImplementedException();
        }
    }
}