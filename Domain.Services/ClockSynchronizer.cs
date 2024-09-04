using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using System.Reactive.Linq;

namespace EtGate.Domain.Services;
// TODO: it may happen that because of locking, there comes a gap between GetDate() and SetDate() thus causing incorrect time be set.
public class ClockSynchronizer
{    
    private readonly IDeviceDate device;
    private readonly Func<DateTimeOffset> dtProvider;
    private readonly Action<DateChanged>? dtChangedNotifier = null;
    private readonly Config config;
    
    IDisposable subscription;

    public record DateChanged(DateTimeOffset origDate, DateTimeOffset modifiedDate);
    public record Config (TimeSpan interval, TimeSpan minThreshold, TimeSpan maxLimit);
    public ClockSynchronizer(IDeviceDate device, Func<DateTimeOffset> dtProvider, Action<DateChanged> dtChangedNotifier, Config config)
    {        
        this.device = device;
        this.dtProvider = dtProvider;
        this.dtChangedNotifier = dtChangedNotifier;
        this.config = config;

        subscription = Observable.Start(() => Synchronize())
                                   .Concat(Observable.Interval(config.interval)
                                                     .Select(_ => Synchronize()))
                                   .Subscribe(); // to trigger the observable to emit as it is a cold observable
    }

    public void Cancel()
    {
        try
        {
            subscription?.Dispose();
        }
        catch
        { }
    }

    private Unit Synchronize()
    {
        var ret = Unit.Default;
        Option<DateTimeOffset> deviceDate = device.GetDate();
        if (deviceDate.IsNone)
            return ret;
        var dtDesired = dtProvider();

        var diff = deviceDate > dtDesired ? deviceDate.Value() - dtDesired : dtDesired - deviceDate.Value();
        if (diff >= config.minThreshold && diff < config.maxLimit)
            if (device.SetDate(dtDesired))
                dtChangedNotifier?.Invoke(new DateChanged(deviceDate.Value(), dtDesired));

        return ret;
    }
}
