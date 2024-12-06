using EtGate.Domain.Services;
using EtGate.Domain.Services.Gate;
using LanguageExt;

namespace EtGate.Devices.Interfaces.Gate;

abstract public class GateControllerBase : //StatusStreamBase<GateHwStatus>, 
    IGateController, IGateInServiceController, IGateModeController, IDeviceDate
{
    // migrate it to the test class where it is needed. Real implmentation is not even using it
    //readonly protected ReplaySubject<EventInNominalMode> EventsInNominalModeSubject = new();

    // 
    abstract public IObservable<EventInNominalMode> InServiceEventsObservable { get; }//=> EventsInNominalModeSubject.AsObservable();

    public abstract bool SetOOS();
    public abstract bool SetEmergency();
    public abstract bool SetMaintenance();
    public abstract bool SetNormalMode(GateOperationConfig config);
    public abstract bool Reboot(bool bHardboot);
    public abstract bool Authorize(int nAuthorizations, bool bEntry);
    public abstract bool SetDate(DateTimeOffset dt);
    public abstract Option<DateTimeOffset> GetDate();
}
