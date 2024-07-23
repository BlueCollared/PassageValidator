using Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Validation
{
    // TODO: Listen to the current site / and other such configuration so as to make pre-checks on the ticket
    public class ValidationMgr : IValidationMgr
    {
        //public List<IValidate> validationSubSystems = new(); better to be direct i.e. OfflineValidationSystem, OnlineValidationSystem
        OfflineValidationSystem offline;
        OnlineValidationSystem online;

        public bool bWorking => throw new NotImplementedException();

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            if (online.IsWorking)
            {
                var result = online.Validate(qrCode);
                if (result != QrCodeValidationResult.CallNotMade)
                    return result;
            }
            else if (offline.IsWorking)
            {
                var result = offline.Validate(qrCode);
                if (result != QrCodeValidationResult.CallNotMade)
                    return result;
            }
            return QrCodeValidationResult.CallNotMade;
        }
    }
}