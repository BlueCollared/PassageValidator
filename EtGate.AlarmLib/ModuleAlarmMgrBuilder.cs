using LanguageExt;

namespace EtGate.AlarmLib;

public class ModuleAlarmMgrBuilder<AlarmType, MetaAlarmType>
 where AlarmType : System.Enum
 where MetaAlarmType : System.Enum
{
    private Dictionary<AlarmType, AlarmLevel> diAlarms = new();
    Dictionary<MetaAlarmType, MetaAlarmConfig<AlarmType>> diMetaAlarms = new();

    public ModuleAlarmMgrBuilder(int moduleId)
    {
        this.moduleId = moduleId;        
    }
    
    static Option<EnumType> MissingEnum<EnumType>(List<EnumType> alarmTypes)
    {
        var missingEnum = Enum.GetValues(typeof(EnumType))
                           .Cast<EnumType>()
                           .FirstOrDefault(alarm => !alarmTypes.Contains(alarm));

        // If no missing value is found, FirstOrDefault will return the default enum value.
        // We need to check if that default value is actually present in the list.
        return alarmTypes.Contains(missingEnum) ? Option<EnumType>.None : missingEnum;
    }

    public Either<string, ModuleAlarmMgr<AlarmType, MetaAlarmType>> Build()
    {        
        var missingAlarm = MissingEnum (diAlarms.Keys.ToList());
        if (!missingAlarm.IsNone)
            return $"{missingAlarm.ToString()} not configured";

        var missingMetaAlarm = MissingEnum(diMetaAlarms.Keys.ToList());
        if (!missingMetaAlarm.IsNone)
            return $"{missingMetaAlarm.ToString()} not configured";

        return new ModuleAlarmMgr<AlarmType, MetaAlarmType>(
            moduleId,
            diAlarms,
            diMetaAlarms
            );
    }

    public void AddAlarm(AlarmType alarmType, AlarmLevel alarmLevel)
    {
        diAlarms.Add(alarmType, alarmLevel);
    }

    public void AddMetaAlarm(
        MetaAlarmType metaAlarmType,
        AlarmLevel alarmLevel,
        params AlarmType[] alarms)
    {
        diMetaAlarms.Add(metaAlarmType, new MetaAlarmConfig<AlarmType> {AlarmLevel=alarmLevel, ConstituentAlarms= alarms});
    }

    public void AddMetaAlarm(
    MetaAlarmType metaAlarmType,
    AlarmLevel alarmLevel
        )
    {
        
    }

    public void SetMetaStatusDefault()
    {
        bSetMetaStatusDefault = true;
    }
    bool bSetMetaStatusDefault = false;
    private readonly int moduleId;    
}
