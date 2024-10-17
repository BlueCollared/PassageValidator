namespace AlarmLib;

static public class AlarmHelper
{
    public static HashSet<T> Alms<T>(params T[] alarms) => alarms.ToHashSet();
    public static HashSet<T> NoAlms<T>() => new HashSet<T>();
}
