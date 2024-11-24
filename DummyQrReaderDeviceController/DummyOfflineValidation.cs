using EtGate.Devices.Interfaces.Validation;
using EtGate.Domain;
using EtGate.Domain.Peripherals.Qr;

namespace DummyQrReaderDeviceController
{
    public class DummyOfflineValidation : OfflineValidationBase
    {
        Task tskStatusDetection;
        bool bStop = false;

        public DummyOfflineValidation()
        {
            //statusSubject.OnNext(new OfflineValidationSystemStatus(DateTimeOffset.MaxValue, DateTimeOffset.Now));
        }

        public override bool Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }


        public override QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}
