using System.Reactive.Linq;

namespace EtGate.Domain.Services.Gate.Functions;

public class DateMgr : IDisposable
{
    public class Config
    {
        public TimeSpan tsFrequencyToCheck;
    }

    private IGateController gateController;
    IDisposable subscription;
    public DateMgr(IGateController gateController, Config config)
    {
        this.gateController = gateController;

        // https://chatgpt.com/c/77dc2c50-88eb-43f1-8c31-780215520436
        SynchronizeDate();
        subscription = Observable.Interval(config.tsFrequencyToCheck).Subscribe(_ => { SynchronizeDate(); });
    }

    void SynchronizeDate()
    {

    }

    public void Dispose()
    {
        subscription.Dispose();
    }
}
