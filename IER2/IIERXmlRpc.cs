using IFS2.Equipment.DriverInterface;
using LanguageExt;
using RetTypOfCommand = LanguageExt.Either<EtGate.IER.IERApiError, EtGate.IER.Success>;

namespace EtGate.IER
{
    public enum IERApiError
    {
        ValueOutOfRange,
        InvalidNumberOfParameters,
        UnexpectedAnswer,
        DeviceRefused,
        DeviceInaccessible,
        UnexpectedException
    };
    public record Success;
    public interface IIerXmlRpc
    {
        RetTypOfCommand SetAuthorisation(int nbpassage, int direction);
        //Option<object[]> ResetAuthorisation(int[] param);
        RetTypOfCommand SetEmergency(bool bEnabled);
        RetTypOfCommand SetMaintenanceMode(bool bEnabled);
        RetTypOfCommand Reboot();
        RetTypOfCommand Restart();
        Option<object[]> ApplyUpdate();
        RetTypOfCommand SetDate(DateTime dt, string timezone = "");
        Either<IERApiError, DateTime> GetDate();
        Either<IERApiError, GetStatusStdRaw> GetStatusStd();
        Either<IERApiError, IERStatus> GetStatus();
        Either<IERApiError, IERSWVersion> GetVersion();
        Option<object[]> GetCounter();
        RetTypOfCommand SetMode(Option<DoorsMode> doorsMode, Option<SideOperatingModes> entry, Option<SideOperatingModes> exit);
        Option<object[]> SetCredentials(string[] param);
        RetTypOfCommand SetTempo(TempoConf conf);
        Either<IERApiError, TempoConf> GetTempo();
        Either<IERApiError, Success> GetSetTempoFlow(TempoFlowConf conf);
        Option<object[]> GetCurrentPassage();
        Option<object[]> SetOutputClient(int[] param);
        Option<object[]> GetMotorSpeed();
        RetTypOfCommand SetMotorSpeed(MotorSpeed param);
        RetTypOfCommand SetBuzzerFraud(int volume, int note);
        RetTypOfCommand SetBuzzerIntrusion(int volume, int note);
        // SetBuzzerIntrusion is not mentioned in the document. Imlies that it must be old        
        Option<object[]> SetBuzzerMode(int[] param);
    }

    public interface IIerStatusMonitor
    {
        void Start();
        IObservable<Either<IERApiError, IERStatus>> StatusObservable { get; }
    }

    public class IERSWVersion
    {
        public string LaneType { get; set; } = "";
        public string SWVersion { get; set; } = "";
        public string CompilationDate { get; set; } = "";
        public string GITVersion { get; set; } = "";
        public string GITDate { get; set; } = "";
    }

    public class TempoConf
    {
        public int FlapRemainOpenPostFreePasses; //Time the obstacles remain open after a user leaves the lane(in Free Mode).
        public int TimeToEnterAfterAuthorisation;              //Time allotted to enter in the lane after an authorisation is granted    public Option<object[]> GetSetTempo(int[] param) 
        public int TimeToValidateAfterDetection;                  //Time allotted to badge after a user is detected in the lane (in Controlled Mode only)        return xmlRpcRaw.GetSetTempo(param);

        public int TimeToCrossAfterDetection; //Time allotted to completely cross the lane after a person is detected.    public Option<object[]> GetSetTempoFlow(int[] param)
        public int TimeAllowedToExitSafetyZone;             //Time allotted to exit the safety zone.    {
        public int TimeAllowedToCrossLaneAfterAuthorisation;         //Time allotted to completely cross the lane after an authorisation is granted        return xmlRpcRaw.GetSetTempoFlow(param);
    }

    public class TempoFlowConf
    {
        // NOTE: it is there in both TempoConf and TempoFlowConf
        public int FlapRemainOpenPostFreePasses; //Time the obstacles remain open after a user leaves the lane(in Free Mode).
        public int FlapRemainOpenPostControlledPasses;  //Time the obstacles remain open after a user leaves the lane(Controlled Mode).
    }

    public class MotorSpeed
    {
        public int StandardOpeningSpeed = 100;
        public int StandardClosingSpeed = 100;
        public int SecurityOpeningSpeed = 20;
        public int DisappearanceSpeed = 20;
        public int FraudClosingSpeed = 100;
        public int SecurityFraudClosingSpeed = 20;
    }

}
