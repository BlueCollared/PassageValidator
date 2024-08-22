using Domain.Peripherals.Passage;
using EtGate.Domain.Passage.PassageEvts;
using EtGate.Domain.Services.Gate;
using IFS2.Equipment.DriverInterface;
using OneOf;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Devices.IER;

// Infrastructure Layer
public class PasageDeviceController : IPassageController
{
    private readonly Subject<ZoneEvent> _zoneEventsSubject = new Subject<ZoneEvent>();

    public PasageDeviceController()
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

    public bool Authorize(int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public void SetMode(eSideOperatingModeGate entry, eSideOperatingModeGate exit, DoorsMode doorsMode)
    {
        throw new NotImplementedException();
    }

    public void SetOpMode(OpMode mode)
    {
        throw new NotImplementedException();
    }

    public IObservable<ZoneEvent> ZoneEvents
    {
        get { return _zoneEventsSubject.AsObservable(); }
    }

    public IObservable<GateHwStatus> GateStatusObservable => throw new NotImplementedException();

    public IObservable<OneOf<Intrusion, Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone>> PassageStatusObservable => throw new NotImplementedException();

    //IObservable<GateHwStatus> IPassageController.GateStatusObservable => throw new NotImplementedException();

    //IObservable<ZoneEvent> IPassageController.ZoneEvents => throw new NotImplementedException();
}

public class ZoneEvent
{
    public string Zone { get; set; }
}
