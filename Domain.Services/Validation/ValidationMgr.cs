using Domain.Peripherals.Qr;

namespace EtGate.Domain.Services.Validation
{
    public class ValidationMgr : IValidationMgr
    {
        //public List<IValidationSubSystem> validationSubSystems = new();
        OfflineValidationSubsytem offline;

        public bool bWorking => throw new NotImplementedException();

        //public bool bWorking => validationSubSystems.Any(x => x.IsWorking);

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            //foreach(var subSystem in validationSubSystems)
            //    if (subSystem.IsWorking)
            //    {
            //        var validationResult = subSystem.Validate(qrCode);
            //        if (validationResult.bCallMade)
            //            return validationResult;
            //    }
            return new QrCodeValidationResult { bCallMade =  false };
        }
    }
}