namespace EtGate.AlarmLib;

public class ModuleAlarmMgrBuilder<AlarmType, MetaAlarmType>
 where AlarmType : System.Enum
 where MetaAlarmType : System.Enum
{
    private Dictionary<AlarmType, AlarmLevel> diAlarms = new();

    public ModuleAlarmMgr<AlarmType, MetaAlarmType> Build()
    {
        //ModuleAlarmMgr<AlarmType, MetaAlarmType> result = new(diAlarms);
        throw new NotImplementedException();
    }

    public void AddAlarm(AlarmType alarmType, AlarmLevel alarmLevel)
    {
        diAlarms.Add(alarmType, alarmLevel);
    }

    public void AddMetaAlarm(
        MetaAlarmType metaAlarmType, 
        AlarmLevel alarmLevel,
        List<AlarmType> alarms)
    {
        throw new NotImplementedException();
    }

    public void AddMetaAlarm(MetaAlarmType metaAlarmType, AlarmLevel alarmLevel, 
        Func<List<AlarmType>, bool> metaAlarmCalculator)
    {
        throw new NotImplementedException();
    }

    public void SetMetaStatus(string name)
    {
        throw new NotImplementedException();
    }
}
