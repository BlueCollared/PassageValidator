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
        // TODO: inject `IIERXmlRpc` instead
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

    public override Option<DateTimeOffset> GetDate()
    {
        return ier.GetDate().Map(x=>new DateTimeOffset(x)); // TODO: see if we can keep `DateTimeOffset` acrosss the code instead of making this conversion
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

    public override bool SetDate(DateTimeOffset dt)
    {
        return ier.SetDate(dt.DateTime); // TODO: see if we can keep `DateTimeOffset` acrosss the code instead of making this conversion
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
