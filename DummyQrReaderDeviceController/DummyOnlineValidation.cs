using EtGate.Devices.Interfaces.Validation;
using EtGate.Domain;
using EtGate.Domain.Peripherals.Qr;

namespace DummyQrReaderDeviceController
{
    public class DummyOnlineValidation : OnlineValidationBase
    {
        public DummyOnlineValidation()
        {
            //statusSubject.OnNext(new OnlineValidationSystemStatus(true));
        }
        public override bool Start()
        {
            return true;
        }

        public override void Stop()
        {

        }

        public override QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}
