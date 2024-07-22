using Domain.Peripherals.Qr;
using EtGate.Domain;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using Peripherals;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Devices.Interfaces
{
    abstract public class OfflineValidationBase : IPeripheral, IValidationSubSystem
    {        
        protected ReplaySubject<OfflineValidationSystemStatus> statusSubject = new();

        public IObservable<OfflineValidationSystemStatus> offlineStatusObservable => statusSubject.AsObservable()
            .ObserveOn(SynchronizationContext.Current);

        IObservable<ValidataionSubSystemStatus> IValidationSubSystem.statusObservable => offlineStatusObservable;

        public abstract bool Start();
        public abstract void Stop();
        public abstract QrCodeValidationResult Validate(QrCodeInfo qrCode);
    }
}
