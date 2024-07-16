using Domain;
using Domain.Peripherals.Passage;
using OneOf;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace IER2
{
    // Infrastructure Layer
    public class PasageController : IPassageController
    {
        private readonly Subject<ZoneEvent> _zoneEventsSubject = new Subject<ZoneEvent>();

        public PasageController()
        {
            // Simulate SDK interaction
            //Task.Run(async () =>
            //{
            //    await Task.Delay(1000);
            //    _zoneEventsSubject.OnNext(new ZoneEvent { Zone = "A" });
            //    await Task.Delay(1000);
            //    _zoneEventsSubject.OnNext(new ZoneEvent { Zone = "B" });
            //    await Task.Delay(1000);
            //    _zoneEventsSubject.OnNext(new ZoneEvent { Zone = "C" });
            //    await Task.Delay(1000);
            //    _zoneEventsSubject.OnNext(new ZoneEvent { Zone = "D" });
            //    await Task.Delay(1000);
            //    _zoneEventsSubject.OnNext(new ZoneEvent { Zone = "Closed" });
            //});
        }

        public bool Authorize(int cnt)
        {
            // SDK call to permit passenger
            return true;
        }

        public IObservable<Domain.ZoneEvent> ZoneEvents
        {
            get { return _zoneEventsSubject.AsObservable(); }
        }

        public IObservable<Domain.Peripherals.Passage.GateStatus> GateStatusObservable => throw new NotImplementedException();

        public IObservable<OneOf<Intrusion, Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone>> PassageStatusObservable => throw new NotImplementedException();

        //IObservable<ZoneEvent> IPassageController.ZoneEvents => throw new NotImplementedException();
    }

    //public class ZoneEvent
    //{
    //    public string Zone { get; set; }
    //}

}
