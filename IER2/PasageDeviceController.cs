using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate;
using EtGate.IER;
using Horizon.XmlRpc.Client;
using IFS2.Equipment.DriverInterface;
using IFS2.Equipment.HardwareInterface.IERPLCManager;

namespace EtGate.Devices.IER;

public class PasageDeviceController : GateControllerBase
{
    bool bIsConnected = false;
    Ier_To_DomainAdapter ier;

    public PasageDeviceController(string url)
    {
        var xmlRpc = XmlRpcProxyGen.Create<IIERXmlRpcInterface>();
        xmlRpc.Url = url;
        ier = new Ier_To_DomainAdapter(
            new IERXmlRpc(xmlRpc)
            );
    }

    public override bool Authorize(int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public override bool Reboot(bool bHardboot)
    {
        if (!bIsConnected)
            return false;

        if (bHardboot)
            return ier.Reboot();
        else
            return ier.Restart();
    }

    public override void SetMode(eSideOperatingModeGate entry, eSideOperatingModeGate exit, DoorsMode doorsMode)
    {
        throw new NotImplementedException();
    }

    public override void SetOpMode(OpMode mode)
    {
        throw new NotImplementedException();
    }    
}
