using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.AlarmLib;

public class MetaAlarmConfig<AlarmType> where AlarmType : System.Enum
{
    public AlarmLevel AlarmLevel { get; }
    public List<AlarmType> ConstituentAlarms { get; }
    public Dictionary<AlarmType, AlarmLevel> masterAlarmConfig;
}

public class ModuleAlarmMgr <AlarmType, MetaAlarmType> 
     where AlarmType: System.Enum
     where MetaAlarmType : System.Enum
{    
    public Dictionary<AlarmLevel, MetaStatusAlarm> diAlarmMapping = new Dictionary<EtGate.AlarmLib.AlarmLevel, EtGate.AlarmLib.MetaStatusAlarm>()
    {
        {AlarmLevel.Error, MetaStatusAlarm.Error},
        {AlarmLevel.Warning, MetaStatusAlarm.Warning},
        {AlarmLevel.NotConnected, MetaStatusAlarm.NotConnected},
    };

    Dictionary<AlarmType, ReplaySubject<bool>> alarmsObservable = new();
    Dictionary<AlarmType, IObservable<bool>> alarmsObservableDistinct = new();
    Dictionary<MetaAlarmType, IObservable<bool>> metaAlarmsObservableDistinct = new();
    IObservable<MetaStatusAlarm> metaStatusAlarm;
    
    IObservable<(int alarmId, int val)> all;

    internal ModuleAlarmMgr(
        int moduleId,
        Dictionary<AlarmType, AlarmLevel> alarmsConfig, 
        Dictionary<MetaAlarmType, (AlarmLevel, MetaAlarmConfig<AlarmType>)> metaAlarmConfig,
        (List<AlarmType> alarmTypes, List<MetaAlarmType> metaAlarmTypes) metaStatusConfig,
        Action<(int alarmId, int alarmValue)> RaiseAlarm
        )
    {
        List<IObservable<MetaStatusAlarm>> alarmsMetaEq = new();
        foreach (var alarm in alarmsConfig)
        {
            var subj = new ReplaySubject<bool>();
            alarmsObservable.Add(alarm.Key, subj);
            alarmsObservableDistinct.Add(alarm.Key, subj.AsObservable().DistinctUntilChanged());
            alarmsMetaEq.Add(alarmsObservableDistinct[alarm.Key].Select(x => x==true? diAlarmMapping[alarm.Value] : MetaStatusAlarm.Normal));
        }

        foreach(var metaAlarm in metaAlarmConfig)
        {
            var constitutentAlarms =
            alarmsObservableDistinct
                .Where(x => x.Key.Equals(metaAlarm.Key));

            var observables = new List<IObservable<bool>>();
            foreach (var constAlarm in constitutentAlarms)
            {
                observables.Add(constAlarm.Value);
                
                var combined = observables
                    .CombineLatest(values => values.Aggregate((acc, current) => acc || current));

                metaAlarmsObservableDistinct.Add(metaAlarm.Key, combined.AsObservable().DistinctUntilChanged());
                alarmsMetaEq.Add(metaAlarmsObservableDistinct[metaAlarm.Key].Select(x => x == true ? diAlarmMapping[metaAlarm.Value.Item1] : MetaStatusAlarm.Normal));
            }
        }
        
        List<IObservable<(int, int)>> lst = new();

        foreach(var alarm in alarmsObservableDistinct)        
            lst.Add(alarm.Value.Select(x => (Convert.ToInt32(alarm.Key), x ? 1: 0)));

        foreach (var alarm in metaAlarmsObservableDistinct)
            lst.Add(alarm.Value.Select(x => (MEATAALARM_BEGINSWITH + Convert.ToInt32(alarm.Key), x ? 1 : 0)));

        var metaStatusStream =
        Observable.CombineLatest(alarmsMetaEq).Select(joinedAlarms => {
            if (joinedAlarms.Contains(MetaStatusAlarm.NotConnected))
                return MetaStatusAlarm.NotConnected;
            else if (joinedAlarms.Contains(MetaStatusAlarm.Error))
                return MetaStatusAlarm.Error;
            else if (joinedAlarms.Contains(MetaStatusAlarm.Warning))
                return MetaStatusAlarm.Warning;
            else
                return MetaStatusAlarm.Normal;
        }).Select(x=>(METASTATUS_ID, (int)x));
        
        all = Observable.Merge(lst);
        all = all.Merge(metaStatusStream)
            .Select(x=>(moduleId*1000 + x.Item1, x.Item2));
    }

    const int METASTATUS_ID = 900;
    const int MEATAALARM_BEGINSWITH = 100;

    public void RaiseAlarm (AlarmType alarmType)
    {
        alarmsObservable[alarmType].OnNext(true);
    }

    public void ClearAlarm (AlarmType alarmType)
    {
        alarmsObservable[alarmType].OnNext(false);
    }
}