
using IFS2.Equipment.HardwareInterface.IERPLCManager;

namespace EtGate.IER;

internal class IERXmlRpcRaw : IIERXmlRpcRaw
{
    private readonly IIERXmlRpcInterface worker;

    public IERXmlRpcRaw(IIERXmlRpcInterface worker)
    {
        this.worker = worker;
    }
    public object[] ApplyUpdate()
    {
        try
        {
            return worker.ApplyUpdate();
        }
        catch (System.Net.WebException )
        {
            MarkDisconnected();
        }
        return null;
    }

    private void MarkDisconnected()
    {
        throw new NotImplementedException();
    }

    public object[] GetCounter()
    {
        try
        {
            return worker.GetCounter();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] GetCurrentPassage()
    {
        try
        {
            return worker.GetCurrentPassage();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] GetDate()
    {
        try
        { 
        return worker.GetDate();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] GetMotorSpeed()
    {
        try {
        return worker.GetMotorSpeed();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] GetSetTempo(int[] param)
    {
        try { 
        return worker.GetSetTempo();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] GetSetTempoFlow(int[] param)
    {
        try
        { 
        return worker.GetSetTempoFlow();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object GetStatusEx()
    {
        try {
        return worker.GetStatusStd(); // TODO: there are two versions GetStatusStd and GetStatus
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] GetVersion()
    {
        try { 
        return worker.GetVersion();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] Reboot()
    {
        try {
        return worker.SendReboot();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] Restart()
    {
        try { 
        return worker.SendRestart(); // TODO: 
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetAuthorization(int[] param)
    {
        try { 
        return worker.SetAuthorisation(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetBuzzerFraud(int[] param)
    {
        try { 
        return worker.SetBuzzerFraud(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetBuzzerIntrusion(int[] param)
    {
        try { 
        return worker.SetBuzzerIntrusion(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetBuzzerMode(int[] param)
    {
        try { 
        return worker.SetBuzzerMode(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetCredentials(string[] param)
    {
        try { 
        return worker.SetCredentials(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetDate(object[] param)
    {
        try { 
        return worker.SetDate(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetEmergency(int[] param)
    {
        try { 
        return worker.SetEmergency(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetMaintenanceMode(int[] param)
    {
        try { 
        return worker.SetMaintenanceMode(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetMode(string[] param)
    {
        try { 
        return worker.SetMode(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetMotorSpeed(object[] param)
    {
        try { 
        return worker.SetMotorSpeed(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }

    public object[] SetOutputClient(int[] param)
    {
        try { 
        return worker.SetOutputClient(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return null;
    }
}
