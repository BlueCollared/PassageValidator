namespace EtGate.Domain
{
    public enum Mode
    {
        AppBooting,
        InService,
        OOS,
        OOO,
        Maintenance, // going to Maintenance means showing the Login page
        Emergency
    };

    public enum OpMode
    {
        InService,
        OOS,
        Emergency
    }

    public class ActiveFunctionalities
    {
        public bool entry;
        public bool exit;
    }
}
