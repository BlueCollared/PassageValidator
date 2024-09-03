using EtGate.Devices.Interfaces.Gate;
using EtGate.IER;
using Horizon.XmlRpc.Client;
using IFS2.Equipment.DriverInterface;
using IFS2.Equipment.HardwareInterface.IERPLCManager;
using LanguageExt;

namespace EtGate.Devices.IER;

public class IerController : GateControllerBase
{
    bool bIsConnected = false;
    Ier_To_DomainAdapter ier;

    public IerController(string url)
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

    public override bool SetEmergency()
    {
        if (!bIsConnected)
            return false;
        return ier.SetEmergency();
    }

    public override bool SetMaintenance()
    {
        if (!bIsConnected)
            return false;
        return ier.SetMaintenance();
    }

    public override bool SetNormalMode(Option<SideOperatingModes> entry, Option<SideOperatingModes> exit)
    {
        throw new NotImplementedException();
    }
}
