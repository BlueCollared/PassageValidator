using Domain.Peripherals.Qr;
using EtGate.Devices.Interfaces;
using EtGate.Domain;
using EtGate.Domain.ValidationSystem;

namespace DummyQrReaderDeviceController
{
    public class DummyOfflineValidation : OfflineValidationBase
    {
        Task tskStatusDetection;
        bool bStop = false;

        public DummyOfflineValidation()
        {
            statusSubject.OnNext(new OfflineValidationSystemStatus(DateTimeOffset.MaxValue, DateTimeOffset.Now));
        }
        public override bool Start()
        {
            return true;
        }

        public override void Stop()
        {
            bStop = true;            
        }

        public override QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }
}
