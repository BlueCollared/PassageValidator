using EtGate.Devices.Interfaces.Gate;
using EtGate.Domain.Services.Gate;
using IFS2.Equipment.DriverInterface;
using IFS2.Equipment.HardwareInterface.IERPLCManager;

namespace EtGate.Devices.IER;

public class PasageDeviceController : GateControllerBase
{
    bool bIsConnected = false;
    CIERRpcHelper ier;

    public PasageDeviceController(string ipAddress, int portNum)
    {
        ier = new CIERRpcHelper(ipAddress, portNum.ToString());
    }

    public override bool Authorize(int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public override bool Reboot(bool bHardboot)
    {
        if (!bIsConnected)
            return false;

        try
        {
            object[] result = ier.Reboot(bHardboot);
            if (int.Parse(result[0].ToString()) > 0)
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }
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
