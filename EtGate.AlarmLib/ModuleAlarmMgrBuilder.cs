namespace EtGate.AlarmLib;

public class ModuleAlarmMgrBuilder<AlarmType, MetaAlarmType>
 where AlarmType : System.Enum
 where MetaAlarmType : System.Enum
{
    private Dictionary<AlarmType, AlarmLevel> diAlarms = new();
    Dictionary<MetaAlarmType, MetaAlarmConfig<AlarmType>> diMetaAlarms = new();

    public ModuleAlarmMgrBuilder(int moduleId, Action<int, int> RaiseAlarm)
    {
        this.moduleId = moduleId;
        raiseAlarm = RaiseAlarm;
    }
    
    public ModuleAlarmMgr<AlarmType, MetaAlarmType> Build()
    {
        return new(
            moduleId,
            diAlarms,
            diMetaAlarms,
            (null, null),
            raiseAlarm
            );
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
        diMetaAlarms.Add(metaAlarmType, new MetaAlarmConfig<AlarmType> {AlarmLevel=alarmLevel, ConstituentAlarms= alarms});
    }   

    public void SetMetaStatusDefault()
    {
        bSetMetaStatusDefault = true;
    }
    bool bSetMetaStatusDefault = false;
    private readonly int moduleId;
    private readonly Action<int, int> raiseAlarm;
}
