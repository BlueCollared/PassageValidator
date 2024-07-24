using Domain.Peripherals.Qr;
using EtGate.Domain.ValidationSystem;
using System.Diagnostics.Metrics;

namespace EtGate.Domain.Services.Validation
{
    // TODO: Listen to the current site / and other such configuration so as to make pre-checks on the ticket
    public class ValidationMgr //: IValidationMgr
    {
        public ValidationMgr(OfflineValidationSystem offline, OnlineValidationSystem online)
        {
            this.offline = offline;
            this.online = online;
        }

        // better to be direct i.e.OfflineValidationSystem, OnlineValidationSystem
        //public List<IValidate> validationSubSystems = new(); 


        OfflineValidationSystem offline;
        OnlineValidationSystem online;

        public IObservable<ValidationSystemStatus> statusObservable;

        public bool bWorking => online.IsWorking || offline.IsWorking;

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