using EtGate.DependencyInjection;

namespace EtGate.MainApp;

public class AppConfig
{
    public GateDepNature gate { get; set; }
    public QrDepNature qr { get; set; }
    public ValidationDepNature validation { get; set; }
    public OtherConf OtherConf { get; set; } = new();
}
