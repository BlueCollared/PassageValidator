using Domain.Peripherals.Passage;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Gate;
using LanguageExt;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.Devices.Interfaces.Gate;

abstract public class GateControllerBase : StatusStreamBase<GateHwStatus>, IGateController, IPassageController, IGateModeController, IDeviceDate
{
    //readonly protected ReplaySubject<GateHwStatus> GateStatusSubject = new();
    //virtual public IObservable<GateHwStatus> GateStatusObservable => GateStatusSubject.AsObservable();

    readonly protected ReplaySubject<EventInNominalMode> EventsInNominalModeSubject = new();
    virtual public IObservable<EventInNominalMode> PassageStatusObservable => EventsInNominalModeSubject.AsObservable();

    public abstract bool SetOOS();
    public abstract bool SetEmergency();
    public abstract bool SetMaintenance();
    public abstract bool SetNormalMode(GateOperationConfig config);
    public abstract bool Reboot(bool bHardboot);
    public abstract bool Authorize(int nAuthorizations);
    public abstract bool SetDate(DateTimeOffset dt);
    public abstract Option<DateTimeOffset> GetDate();    
}
