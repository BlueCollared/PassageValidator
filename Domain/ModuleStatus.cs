namespace EtGate.Domain
{
    //abstract public class ModuleStatus
    //{
    //    // TODO: There is no need of this field in the base class. We hardly get any benefit. The client isn't going to access it from the base class
    //    public virtual bool IsAvailable { get; protected set; }
    //}

    public interface ModuleStatus
    {
        // TODO: There is no need of this field in the base class. We hardly get any benefit. The client isn't going to access it from the base class
        bool IsAvailable { get; }
    }
}