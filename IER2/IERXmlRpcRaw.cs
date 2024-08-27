
using IFS2.Equipment.HardwareInterface.IERPLCManager;
using LanguageExt;

namespace EtGate.IER;

public class IERXmlRpcRaw : IIERXmlRpcRaw
{
    private readonly IIERXmlRpcInterface worker;
    readonly Option<object[]> none = Option<object[]>.None;

    public IERXmlRpcRaw(IIERXmlRpcInterface worker)
    {
        this.worker = worker;
    }
    public Option<object[]> ApplyUpdate()
    {
        try
        {
            return worker.ApplyUpdate();
        }
        catch (System.Net.WebException )
        {
            MarkDisconnected();
        }
        return none;
    }

    private void MarkDisconnected()
    {
        throw new NotImplementedException();
    }

    public Option<object[]> GetCounter()
    {
        try
        {
            return worker.GetCounter();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> GetCurrentPassage()
    {
        try
        {
            return worker.GetCurrentPassage();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> GetDate()
    {
        try
        { 
        return worker.GetDate();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> GetMotorSpeed()
    {
        try {
        return worker.GetMotorSpeed();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> GetSetTempo(int[] param)
    {
        try { 
        return worker.GetSetTempo();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> GetSetTempoFlow(int[] param)
    {
        try
        { 
        return worker.GetSetTempoFlow();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> GetVersion()
    {
        try { 
        return worker.GetVersion();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> Reboot()
    {
        try {
        return worker.SendReboot();
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return Option<object[]>.None;
    }

    public Option<object[]> Restart()
    {
        try { 
        return worker.SendRestart(); // TODO: 
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return Option<object[]>.None;
    }

    public Option<object[]> SetAuthorisation(int[] param)
    {
        try { 
        return worker.SetAuthorisation(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetBuzzerFraud(int[] param)
    {
        try { 
        return worker.SetBuzzerFraud(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetBuzzerIntrusion(int[] param)
    {
        try { 
        return worker.SetBuzzerIntrusion(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }
    

    public Option<object[]> SetBuzzerMode(int[] param)
    {
        try { 
        return worker.SetBuzzerMode(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetCredentials(string[] param)
    {
        try { 
        return worker.SetCredentials(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetDate(object[] param)
    {
        try { 
        return worker.SetDate(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetEmergency(int[] param)
    {
        try { 
        return worker.SetEmergency(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetMaintenanceMode(int[] param)
    {
        try { 
        return worker.SetMaintenanceMode(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetMode(string[] param)
    {
        try { 
        return worker.SetMode(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetMotorSpeed(object[] param)
    {
        try { 
        return worker.SetMotorSpeed(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object[]> SetOutputClient(int[] param)
    {
        try { 
        return worker.SetOutputClient(param);
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Option<object> GetStatusEx()
    {
        try
        {
            return worker.GetStatusStd(); // TODO: there are two versions GetStatusStd and GetStatus
        }
        catch (System.Net.WebException)
        {
            MarkDisconnected();
        }
        return Option<object>.None;

    }
}
