
using IFS2.Equipment.HardwareInterface.IERPLCManager;
using LanguageExt;
using OneOf;
using System;
using System.Net;

using IERApiResult = LanguageExt.Either<EtGate.IER.IERApiError, object[]>;


namespace EtGate.IER;

// TODO: this API is bad. It should return parsed structures rather than object[].
public class IERXmlRpc : IIERXmlRpc
{
    private readonly IIERXmlRpcInterface worker;
    readonly Option<object[]> none = Option<object[]>.None;

    public IERXmlRpc(IIERXmlRpcInterface worker)
    {
        this.worker = worker;
    }

    #region Helper
    static readonly Func<object[], IERApiResult> CheckNumberOfIpParams = (result) =>
                  result.Length == 1
                && result[0] is int
                && (int)result[0] == 0 ? IERApiError.InvalidNumberOfParameters : result;

    static readonly Func<object[], IERApiResult> CheckZeroethParam_Minus1 = (result) =>
              result.Length == 1
            && result[0] is int
            && (int)result[0] == -1 ? IERApiError.DeviceRefused : result;

    static readonly Func<object[], IERApiResult> CheckZeroethParam_0 = (result) =>
              result.Length == 1
            && result[0] is int
            && (int)result[0] == 0 ? IERApiError.DeviceRefused : result;

    static readonly Func<object[], Either<IERApiError, Success>> CheckZeroethParam_1_MeansSuccess = (result) =>
              (result.Length == 1
            && result[0] is int
            && (int)result[0] == 1) ? new Success() : IERApiError.UnexpectedAnswer;

    static readonly Func<object[], Either<IERApiError, Success>> TristateChecker = (result) =>
    {
        if (result.Length != 1)
            return IERApiError.UnexpectedAnswer;
        if (!(result[0] is int))
            return IERApiError.UnexpectedAnswer;
        int r = (int)result[0];
        switch (r)
        {
            case -1:
                return IERApiError.ValueOutOfRange;
            case 1:
                return new Success();
            case 0:
                return IERApiError.InvalidNumberOfParameters;
            default:
                return IERApiError.UnexpectedAnswer;
        }
    };


    static readonly Func<object[], int, IERApiResult> InputOutOfRange = (result, nMinusOnes) =>
    {
        if (result.Length != nMinusOnes)
            return result;
        foreach (object o in result)
            if (!(o is int) || (Int32)o != -1)
                return result;
        return IERApiError.ValueOutOfRange;
    };
    #endregion

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
         : IERApiError.UnexpectedAnswer;
            }
            catch
            {
                return IERApiError.UnexpectedAnswer;
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

    IERApiResult MakeCall(Func<object[]> act)
    {
        try
        {
            return act();
        }
        catch(WebException exp)
        {
            return IERApiError.DeviceInaccessible;
        }
    }

    public Either<IERApiError, Success> SetTempo(TempoConf conf)
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
            .Bind(x => Either<IERApiError, Success>.Right(new Success()));
    }

    public Either<IERApiError, TempoConf> GetTempo()
    {
        try
        {
            var result = worker.GetSetTempo();
            if (CheckNumberOfIpParams(result))
                return IERApiError.InvalidNumberOfParameters;
            else if (InputOutOfRange(result, 2))
                return IERApiError.ValueOutOfRange;
            throw new NotImplementedException();
        }
        catch (WebException)
        {
            MarkDisconnected();
            return IERApiError.DeviceInaccessible;
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

    public Either<IERApiError, IERSWVersion> GetVersion()
    {
        Func<object[], Either<IERApiError, IERSWVersion>> versionExtract = (object[] op) =>
        {
            try
            {
                if (op.Length != 5)
                    return IERApiError.UnexpectedAnswer;

                return new IERSWVersion
                {
                    LaneType = (string)op[0],
                    SWVersion = (string)op[1],
                    CompilationDate = (string)op[2],
                    GITVersion = (string)op[3],
                    GITDate = (string)op[4]
                };
            }
            catch
            {
                return IERApiError.UnexpectedAnswer;
            }
        };
        return MakeCall(() => worker.GetVersion())
            .Bind(versionExtract);
    }

    public Either<IERApiError, Success> Reboot()
    {
        return MakeCall(() => worker.SendReboot())
            .Bind(CheckZeroethParam_Minus1)
            .Bind(CheckZeroethParam_1_MeansSuccess);
    }

    public Either<IERApiError, Success> Restart()
    {
        return MakeCall(() => worker.SendRestart())
            .Bind(CheckZeroethParam_1_MeansSuccess);
    }

    public Either<IERApiError, Success> SetAuthorisation(int nbpassage, int direction)
    {
        int[] param = new int[] { 0, 0, 0, 0, 0, 0 };
        switch (direction)
        {
            case 0:
                param[0] = nbpassage;
                break;
            case 1:
                param[1] = nbpassage;
                break;
        }
        return MakeCall(() => worker.SetAuthorisation(param))
            .Bind(CheckZeroethParam_Minus1)
            .Bind(CheckZeroethParam_1_MeansSuccess);
    }

    public Either<IERApiError, Success> SetBuzzerFraud(int volume, int note)
    {
        int[] param = new int[] { volume, note};
        return MakeCall(() => worker.SetBuzzerFraud(param))
            .Bind(TristateChecker);
    }
    public Either<IERApiError, Success> SetBuzzerIntrusion(int volume, int note)
    {
        int[] param = new int[] { volume, note };
        return MakeCall(() => worker.SetBuzzerIntrusion(param))
            .Bind(TristateChecker);
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

    public Either<IERApiError, Success> SetDate(DateTime dt, string timezone = "")
    {
        object[] dtparams = new object[7];
        dtparams[0] = (int)dt.Year;
        dtparams[1] = (int)dt.Month;
        dtparams[2] = (int)dt.Day;
        dtparams[3] = (int)dt.Hour;
        dtparams[4] = (int)dt.Minute;
        dtparams[5] = (int)dt.Second;
        dtparams[6] = (string)timezone;// dt.timezone;

        return MakeCall(() => worker.SetDate(dtparams))
            .Bind(CheckNumberOfIpParams)
            .Bind(CheckZeroethParam_Minus1)            
            .Bind(CheckZeroethParam_1_MeansSuccess);
    }

    public Either<IERApiError, Success> SetEmergency(bool bEnabled)
    {
        int[] param = { bEnabled? 1: 0 };
        return MakeCall(() => worker.SetEmergency(param))
            .Bind(TristateChecker);
    }

    public Either<IERApiError, Success> SetMaintenanceMode(bool bEnabled)
    {
        int[] param = { bEnabled ? 1 : 0 };
        return MakeCall(() => worker.SetEmergency(param))
            .Bind(TristateChecker);
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

    public Option<object> GetStatusFull()
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

    public Either<IERApiError, IERStatus> GetStatus()
    {
        Func<object[], Either<IERApiError, IERStatus>> statusExtract = (object[] resp) =>
        {
            throw new NotImplementedException();
            try
            {
            }
            catch
            {
                return IERApiError.UnexpectedAnswer;
            }
        };
        return MakeCall(() => worker.GetStatus())
            .Bind(statusExtract);            
    }
}
