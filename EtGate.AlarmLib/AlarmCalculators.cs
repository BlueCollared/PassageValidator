namespace EtGate.AlarmLib;

internal static class AlarmCalculators<AlarmType>  where AlarmType : System.Enum
{
    public static bool MetaAlarmCalculatorDefault(        
        Dictionary<AlarmType, bool> alarmStatus
        )
        =>alarmStatus.Values.Contains(true);
    

    public static MetaStatusAlarm MetaStatusCalculatorDefault(Dictionary<AlarmType, AlarmLevel> alarmConfig,
        Dictionary<AlarmType, bool> alarmStatus)
    {
        var activeAlarms = alarmStatus.Where(x => x.Value == true);
        var joinedAlarms = alarmConfig
            .Join(activeAlarms,
          config => config.Key,
          status => status.Key,
          (config, status) => new
          {
              //AlarmType = config.Key,
              AlarmLevel = config.Value            
          });
        if (joinedAlarms.Any(x => x.AlarmLevel == AlarmLevel.NotConnected))
            return MetaStatusAlarm.NotConnected;
        else if (joinedAlarms.Any(x => x.AlarmLevel == AlarmLevel.Error))
            return MetaStatusAlarm.Error;
        else if (joinedAlarms.Any(x => x.AlarmLevel == AlarmLevel.Warning))
            return MetaStatusAlarm.Warning;
        else
            return MetaStatusAlarm.Normal;
    }
}