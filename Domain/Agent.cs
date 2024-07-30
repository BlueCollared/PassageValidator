namespace EtGate.Domain
{
    public enum OpTyp { Supervisor, Maintenace };
    public class Agent
    {
        public string id;
        public string name;
        public OpTyp opTyp;
    }
}
