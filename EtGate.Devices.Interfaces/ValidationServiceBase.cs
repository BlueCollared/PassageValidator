using Domain.Peripherals.Qr;
using EtGate.Domain;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using Peripherals;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.QrReader
{
    public abstract class ValidationServiceBase : IPeripheral, IValidationSubSystem
    {
        protected ReplaySubject<ValidationServiceStatus> statusSubject = new();

        public IObservable<ValidationServiceStatus> serviceStatusObservable => statusSubject.AsObservable()
            .ObserveOn(SynchronizationContext.Current);

        IObservable<ValidataionSubSystemStatus> IValidationSubSystem.statusObservable => serviceStatusObservable;

        public abstract bool Start();
        public abstract void Stop();
        public abstract QrCodeValidationResult Validate(QrCodeInfo qrCode);
    }
}