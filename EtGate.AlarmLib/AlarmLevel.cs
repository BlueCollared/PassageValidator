namespace EtGate.AlarmLib;

// --- These values enforce an order which is being used in the code (ModuleAlarmMgr)
public enum MetaStatusAlarm
{
    Normal = 0,
    Warning = 1,
    Error = 2,
    NotConnected = 3,
}
// ------ END  MetaStatusAlarm

public enum AlarmLevel
{
    NotConnected,
    Error,
    Warning
}

public enum MetaAlarmLevel
{
    NotConnected,
    Error,
    Warning
}