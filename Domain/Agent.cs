namespace EtGate.Domain
{
    public enum OpTyp { Supervisor, Maintenace };
    public class Agent
    {
        public string id { get; set; }
        public string name { get; set; }
        public OpTyp opTyp { get; set; }
    }
}
