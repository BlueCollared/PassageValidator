﻿using LanguageExt;
using System.Net;
using System.Runtime.CompilerServices;
using IERApiResult = LanguageExt.Either<EtGate.IER.IERApiError, object[]>;
using IERApiResult2 = LanguageExt.Either<EtGate.IER.IERApiError, object>;


namespace EtGate.IER;

// TODO: this API is bad. It should return parsed structures rather than object[].
public class IERXmlRpc : IIerXmlRpc
{
    private readonly IIERXmlRpcInterface worker;
    readonly Option<object[]> none = Option<object[]>.None;

    public IERXmlRpc(IIERXmlRpcInterface worker, Domain.Services.ILogger logger)
    {
        this.logger = logger;
        this.worker = worker;
    }

    #region Helper
    static readonly Dictionary<SideOperatingMode, string> diSideOperatingMode = new Dictionary<SideOperatingMode, string>{
        { SideOperatingMode.Closed, "Closed"},
        { SideOperatingMode.Controlled, "Controlled"},
        { SideOperatingMode.Free, "Free"}
    };

    static readonly Dictionary<DoorsMode, string> diDoorsMode = new Dictionary<DoorsMode, string> {
        {DoorsMode.LockClosed, "BlockClosed" },
        {DoorsMode.NormallyClosed, "Nc" },
        {DoorsMode.NormallyOpenedA, "Noa" },
        {DoorsMode.NormallyOpenedB, "Nob" },
        {DoorsMode.OpticalA, "OpticalA" },
        {DoorsMode.OpticalB, "OpticalB" },
        {DoorsMode.LockedOpenA, "LockedOpenA" },
        {DoorsMode.LockedOpenB, "LockedOpenB" }
     };

    static readonly Func<object[], IERApiResult> CheckNumberOfIpParams = (result) =>
                  result.Length == 1
                && result[0] is int xy
                && xy == 0 ? IERApiError.InvalidNumberOfParameters : result;

    static readonly Func<object[], IERApiResult> CheckZeroethParam_Minus1 = (result) =>
              result.Length == 1
            && result[0] is int xy
            && xy == -1 ? IERApiError.DeviceRefused : result;

    static readonly Func<object[], IERApiResult> CheckZeroethParam_0 = (result) =>
              result.Length == 1
            && result[0] is int xy
            && xy == 0 ? IERApiError.DeviceRefused : result;

    static readonly Func<object[], Either<IERApiError, Success>> CheckZeroethParam_1_MeansSuccess = (result) =>
              result.Length == 1
            && result[0] is int xy
            && xy == 1 ? new Success() : IERApiError.UnexpectedAnswer;

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
        catch (WebException)
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
            .Log(logger)
            .Bind(dateExtract);
    }

    public Option<object[]> GetMotorSpeed()
    {
        try
        {
            return worker.GetMotorSpeed();
        }
        catch (WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    IERApiResult2 MakeCall(Func<object> act)
    {
        try
        {
            return act();
        }
        catch (WebException exp)
        {
            return IERApiError.DeviceInaccessible;
        }
        catch (Exception exp)
        {
            return IERApiError.UnexpectedException;
        }
    }
    IERApiResult MakeCall(Func<object[]> act)
    {
        try
        {
            return act();
        }
        catch (WebException exp)
        {
            return IERApiError.DeviceInaccessible;
        }
        catch (Exception exp)
        {
            return IERApiError.UnexpectedException;
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

    public Either<IERApiError, Success> GetSetTempoFlow(TempoFlowConf conf)
    {
        int[] param2 = new int[2];
        param2[0] = conf.FlapRemainOpenPostFreePasses; //Time the obstacles remain open after a user leaves the lane(in Free Mode).
        param2[1] = conf.FlapRemainOpenPostControlledPasses;  //Time the obstacles remain open after a user leaves the lane(Controlled Mode).

        throw new NotImplementedException();

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
        int[] param = new int[] { volume, note };
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
        try
        {
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
        try
        {
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
        int[] param = { bEnabled ? 1 : 0 };
        return MakeCall(() => worker.SetEmergency(param))
            .Bind(TristateChecker);
    }

    public Either<IERApiError, Success> SetMaintenanceMode(bool bEnabled)
    {
        int[] param = { bEnabled ? 1 : 0 };
        return MakeCall(() => worker.SetEmergency(param))
            .Bind(TristateChecker);
    }

    private Domain.Services.ILogger logger;

    public Either<IERApiError, Success> SetMode(Option<DoorsMode> doorsMode, Option<SideOperatingMode> entry, Option<SideOperatingMode> exit)
    {

        string[] param = { "", "", "" };
        doorsMode.IfSome(x => { param[0] = diDoorsMode[x]; });
        entry.IfSome(x => { param[1] = diSideOperatingMode[x]; });
        exit.IfSome(x => { param[2] = diSideOperatingMode[x]; });

        return MakeCall(() => worker.SetMode(param))
            .Bind(TristateChecker);
    }

    public Either<IERApiError, Success> SetMotorSpeed(MotorSpeed conf)
    {
        object[] param3 = new object[7];
        param3[0] = "Entry";    // TODO: for now, copied from SGP
        param3[1] = conf.StandardOpeningSpeed;
        param3[2] = conf.StandardClosingSpeed;
        param3[3] = conf.SecurityOpeningSpeed;
        param3[4] = conf.DisappearanceSpeed;
        param3[5] = conf.FraudClosingSpeed;
        param3[6] = conf.SecurityFraudClosingSpeed;

        return MakeCall(() => worker.SetMotorSpeed(param3))
            .Bind(TristateChecker);
    }

    public Option<object[]> SetOutputClient(int[] param)
    {
        try
        {
            return worker.SetOutputClient(param);
        }
        catch (WebException)
        {
            MarkDisconnected();
        }
        return none;
    }

    public Either<IERApiError, GetStatusStdRaw> GetStatusStd()
    {
        return MakeCall(() => worker.GetStatusStd())
            .Bind(result =>
            IERRHelper.ProcessGetStatusStd(result)
                .MapLeft(x => IERApiError.UnexpectedAnswer));
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

static class Helper
{
    public static IERApiResult Log(this IERApiResult either, Domain.Services.ILogger logger, [CallerMemberName] string caller = null)
    {
        either.Match(
            Right: r =>
            {
                Console.WriteLine($"Right: {r}");
            },
            Left: l =>
            {
                Console.WriteLine($"Left: {l}");
            });
        return either;
    }
}