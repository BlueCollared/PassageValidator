namespace EtGate.Domain
{
    public abstract record ModuleStatus
    {
        // TODO: There is no need of this field in the base class. We hardly get any benefit. The client isn't going to access it from the base class
        abstract public bool IsAvailable { get; }
        //public abstract ModuleStatus defStatus { get; }
        //bool StatusKnown = false;
    }
}