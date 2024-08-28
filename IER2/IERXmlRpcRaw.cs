
using IFS2.Equipment.HardwareInterface.IERPLCManager;
using LanguageExt;
using System.Net;

using IERApiResult = LanguageExt.Either<EtGate.IER.IERApiError, object[]>;

namespace EtGate.IER;

// TODO: this API is bad. It should return parsed structures rather than object[].
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
        catch (WebException )
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
        catch (WebException)
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
        catch (WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Either<IERApiError, DateTime> GetDate()
    {
        Func<object[], Either<IERApiError, DateTime>> dateExtract = (object[] resp) =>
        {
            try
            {
                return resp.Length > 7 ?
            (new DateTime
            ((int)resp[0], (int)resp[1], (int)resp[2], (int)resp[3], (int)resp[4], (int)resp[5])).AddSeconds((int)resp[6])
         : IERApiError.bUnexpectedAnswer;
            }
            catch
            {
                return IERApiError.bUnexpectedAnswer;
            }
        };

        return MakeCall(() => worker.GetDate())
            .Bind(dateExtract);        
    }

    public Option<object[]> GetMotorSpeed()
    {
        try {
        return worker.GetMotorSpeed();
        }
        catch (WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    static readonly Func<object[], IERApiResult> CheckNumberOfIpParams = (result) => 
                  result.Length == 1
                && result[0] is int
                && (int)result[0] == 0 ? IERApiError.bInvalidNumberOfParameters : result;

    static readonly Func<IERApiResult, IERApiResult> IsNumberOfParamsIncorrectX = (ip)=>
        ip.Bind(CheckNumberOfIpParams);
    
      
    static readonly Func<object[], int, IERApiResult> InputOutOfRange = (result, nMinusOnes) =>
    {
        if (result.Length != nMinusOnes)
            return result;
        foreach (object o in result)
            if (!(o is int) || (Int32)o != -1)
                return result;
        return IERApiError.bValueOutOfRange;
    };

    IERApiResult MakeCall(Func<object[]> act)
    {
        try
        {
            return act();
        }
        catch(WebException exp)
        {
            return IERApiError.bDeviceInaccessible;
        }
    }

    public Either<IERApiError, bool> SetTempo(TempoConf conf)
    {
        int[] param =
        [
            conf.FlapRemainOpenPostFreePasses,
            -1, // not int use
            conf.TimeToEnterAfterAuthorisation,
            -1,// not in use
            conf.TimeToValidateAfterDetection,
            -1,// not int use
            conf.TimeToCrossAfterDetection,
            conf.TimeAllowedToExitSafetyZone,
            conf.TimeAllowedToCrossLaneAfterAuthorisation,
        ];
        
        return MakeCall(() => worker.GetSetTempo(param))
            .Bind(CheckNumberOfIpParams)
            .Bind(x => InputOutOfRange(x, param.Length))
            .Bind(x => Either<IERApiError, bool>.Right(true));
    }

    public Either<IERApiError, TempoConf> GetTempo()
    {
        try
        {
            var result = worker.GetSetTempo();
            if (CheckNumberOfIpParams(result))
                return IERApiError.bInvalidNumberOfParameters;
            else if (InputOutOfRange(result, 2))
                return IERApiError.bValueOutOfRange;
            throw new NotImplementedException();
        }
        catch (WebException)
        {
            MarkDisconnected();
            return IERApiError.bDeviceInaccessible;
        }
    }

    public Option<object[]> GetSetTempoFlow(int[] param)
    {
        try
        { 
        return worker.GetSetTempoFlow();
        }
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
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
        catch (WebException)
        {
            MarkDisconnected();
        }
        return Option<object>.None;

    }

    public Option<IERStatus> GetStatus()
    {
        throw new NotImplementedException();
    }
}
