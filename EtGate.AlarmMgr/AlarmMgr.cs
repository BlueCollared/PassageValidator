using Equipment.Core.Message;
using EtGate.Domain.Peripherals.Qr;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace EtGate.AlarmMgr;

public class AlarmMgr : IAlarmMgr
{
    IScheduler scheduler = new EventLoopScheduler();
    public AlarmMgr(DeviceStatusSubscriber<QrReaderStatus> evtQrReaderStatus)
    {
        evtQrReaderStatus.Messages
            .SubscribeOn(scheduler)
            .Subscribe(x => { });
    }
}

public interface IAlarmMgr
{
}