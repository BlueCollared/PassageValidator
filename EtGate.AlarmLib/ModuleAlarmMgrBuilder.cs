using LanguageExt;

namespace EtGate.AlarmLib;

public class ModuleAlarmMgrBuilder<AlarmType, MetaAlarmType>
 where AlarmType : System.Enum
 where MetaAlarmType : System.Enum
{
    private Dictionary<AlarmType, AlarmLevel> diAlarms = new();
    Dictionary<MetaAlarmType, MetaAlarmConfig<AlarmType, MetaAlarmType>> diMetaAlarms = new();

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

        Dictionary<MetaAlarmType, System.Collections.Generic.HashSet<MetaAlarmType>> deps = new();

        foreach (var kvp in diMetaAlarms)
            deps.Add(kvp.Key, kvp.Value.ConstituentMetaAlarms);


        if (HasCycle(deps))
            return "Cycles in the input";

        return new ModuleAlarmMgr<AlarmType, MetaAlarmType>(
            moduleId,
            diAlarms,
            diMetaAlarms
            );
    }

    private enum NodeState
    {
        Unvisited,
        Visiting,
        Visited
    }

    public static bool HasCycle(Dictionary<MetaAlarmType, System.Collections.Generic.HashSet<MetaAlarmType>> diMetaAlarms)
    {
        var visited = new Dictionary<MetaAlarmType, NodeState>();

        // Initialize all nodes as unvisited
        foreach (var node in diMetaAlarms.Keys)
        {
            visited[node] = NodeState.Unvisited;
        }

        // Perform DFS for each unvisited node
        foreach (var node in diMetaAlarms.Keys)
        {
            if (visited[node] == NodeState.Unvisited)
            {
                if (DFS(node, diMetaAlarms, visited))
                {
                    return true; // Cycle detected
                }
            }
        }

        return false; // No cycle detected
    }

    private static bool DFS(
        MetaAlarmType currentNode,
        Dictionary<MetaAlarmType, System.Collections.Generic.HashSet<MetaAlarmType>> diMetaAlarms,
        Dictionary<MetaAlarmType, NodeState> visited)
    {
        // Mark the current node as visiting
        visited[currentNode] = NodeState.Visiting;

        // Visit all neighbors
        if (diMetaAlarms.TryGetValue(currentNode, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (visited[neighbor] == NodeState.Unvisited)
                {
                    // Recur for unvisited neighbor
                    if (DFS(neighbor, diMetaAlarms, visited))
                    {
                        return true; // Cycle detected
                    }
                }
                else if (visited[neighbor] == NodeState.Visiting)
                {
                    // Cycle detected
                    return true;
                }
            }
        }

        // Mark the current node as visited
        visited[currentNode] = NodeState.Visited;
        return false;
    }

    public void AddAlarm(AlarmType alarmType, AlarmLevel alarmLevel)
    {
        diAlarms.Add(alarmType, alarmLevel);
    }

    public void AddMetaAlarm(
        MetaAlarmType metaAlarmType,
        AlarmLevel alarmLevel,
        System.Collections.Generic.HashSet<AlarmType> alarms,
        System.Collections.Generic.HashSet<MetaAlarmType> metaAlarms)
    {
        diMetaAlarms.Add(metaAlarmType, new MetaAlarmConfig<AlarmType, MetaAlarmType> {AlarmLevel=alarmLevel, ConstituentAlarms= alarms, ConstituentMetaAlarms = metaAlarms});
    }

    public void SetMetaStatusDefault()
    {
        bSetMetaStatusDefault = true;
    }
    bool bSetMetaStatusDefault = false;
    private readonly int moduleId;    
}
