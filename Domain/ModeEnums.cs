namespace Domain
{
    public enum Mode
    {
        AppBooting,
        InService,
        OOS,
        OOO,
        Maintenance,
        WaitForAgentLogin,
        Emergency
    };

    public enum OpMode
    {
        InService,
        OOS,
        Emergency
    }
}
