using LanguageExt.UnsafeValueAccess;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace EtGate.AlarmLib;

public class MetaAlarmConfig<AlarmType, MetaAlarmType> 
    where AlarmType : System.Enum
    where MetaAlarmType : System.Enum
{
    public AlarmLevel? AlarmLevel { get; internal set; }
    public HashSet<AlarmType> ConstituentAlarms { get; internal set; } = new();
    public HashSet<MetaAlarmType> ConstituentMetaAlarms { get; internal set; } = new();
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

    Dictionary<AlarmLevel, MetaStatusAlarm> diMetaAlarmMapping = new Dictionary<AlarmLevel, MetaStatusAlarm> {
        {AlarmLevel.Error, MetaStatusAlarm.Error },
        {AlarmLevel.Warning, MetaStatusAlarm.Warning },
        {AlarmLevel.NotConnected, MetaStatusAlarm.NotConnected },
    };

    Dictionary<AlarmType, ReplaySubject<bool>> alarmsObservable = new();    
    Dictionary<MetaAlarmType, IObservable<bool>> metaAlarmsObservableDistinct = new();
    IObservable<MetaStatusAlarm> metaStatusAlarm;
    
    public IObservable<(int alarmId, int val)> all { get; private set; }

    static bool IsSuperSet<T>(IEnumerable<T> SuperSet, IEnumerable<T> subset)
    {
        return subset.All(item => SuperSet.Contains(item));
    }


    internal ModuleAlarmMgr(
        Dictionary<AlarmType, AlarmLevel> alarmsConfig, 
        Dictionary<MetaAlarmType, MetaAlarmConfig<AlarmType, MetaAlarmType>> metaAlarmConfig
        )
    {
        if (Enum.GetValues(typeof(AlarmLevel)).Length != diAlarmMapping.Count)
            throw new Exception($"{nameof(diAlarmMapping)} not properly populated");

        if (Enum.GetValues(typeof(MetaAlarmLevel)).Length != diMetaAlarmMapping.Count)
            throw new Exception($"{nameof(diMetaAlarmMapping)} not properly populated");

        Dictionary<AlarmType, IObservable<MetaStatusAlarm>> alarmsMetaEq = new();
        Dictionary<MetaAlarmType, IObservable<MetaStatusAlarm>> metaAlarmsMetaEq = new();

        foreach (var alarm in alarmsConfig)
        {
            var subj = new ReplaySubject<bool>();
            alarmsObservable.Add(alarm.Key, subj);
            var xy = subj.AsObservable().DistinctUntilChanged();
            alarmsMetaEq.Add(alarm.Key, xy.Select(x => x==true? diAlarmMapping[alarm.Value] : MetaStatusAlarm.Normal));
        }        

        while( true)
        {
            var metaAlarm =
                metaAlarmConfig.Find(x => !metaAlarmsMetaEq.ContainsKey(x.Key)
                            && IsSuperSet(metaAlarmsMetaEq.Keys.ToList(), x.Value.ConstituentMetaAlarms));

            if (metaAlarm.IsNone)
                break;

            var metaAlarm_ = metaAlarm.Value();
            var constituentAlarmsStreams = metaAlarm_.Value.ConstituentAlarms.Select(x => alarmsMetaEq[x]);
            var constituentMetaAlarmsStreams = metaAlarm_.Value.ConstituentMetaAlarms.Select(x =>metaAlarmsMetaEq[x]);

            var allConstituentStreams = constituentAlarmsStreams.Append(constituentMetaAlarmsStreams);
            var myStaticLevel = metaAlarmConfig[metaAlarm_.Key].AlarmLevel;
            var consolidatedInputStream = allConstituentStreams
                    .CombineLatest()
                    .Select(latestStatuses =>
                    {
                        if (latestStatuses.Contains(MetaStatusAlarm.NotConnected))
                            return MetaStatusAlarm.NotConnected;

                        if (latestStatuses.Contains(MetaStatusAlarm.Error))
                            return MetaStatusAlarm.Error;

                        if (latestStatuses.Contains(MetaStatusAlarm.Warning))
                            return MetaStatusAlarm.Warning;

                        return MetaStatusAlarm.Normal;
                    });

            metaAlarmsMetaEq[metaAlarm_.Key] =
                 consolidatedInputStream.Select(x=> {
                     if (x != MetaStatusAlarm.Error)
                         return x;
                     else
                     {
                         if (myStaticLevel == null)
                             return x;
                         else
                             return diMetaAlarmMapping[(AlarmLevel)myStaticLevel];
                     }
            }).DistinctUntilChanged();
        }
        
        List<IObservable<(int, int)>> lst = new();

        foreach(var alarm in alarmsMetaEq)
            lst.Add(alarm.Value.Select(x => (Convert.ToInt32(alarm.Key), (int)x)));

        foreach (var alarm in metaAlarmsMetaEq)
            lst.Add(alarm.Value.Select(x => (MEATAALARM_BEGINSWITH + Convert.ToInt32(alarm.Key), (int)x)));

        all = Observable.Merge(lst);
        //all = all.Merge(metaStatusStream)
        //    .Select(x=>(moduleId*1000 + x.Item1, x.Item2));        
    }

    //const int METASTATUS_ID = 900;
    public const int MEATAALARM_BEGINSWITH = 100;

    public void RaiseAlarm (AlarmType alarmType)
    {
        alarmsObservable[alarmType].OnNext(true);
    }

    public void ClearAlarm (AlarmType alarmType)
    {
        alarmsObservable[alarmType].OnNext(false);
    }
}