using IFS2.Equipment.HardwareInterface.IERPLCManager;
using LanguageExt;

namespace EtGate.IER
{
    public enum IERApiError
    {
        ValueOutOfRange,
        InvalidNumberOfParameters,
        UnexpectedAnswer,
        DeviceRefused,
        DeviceInaccessible
    };
    public record Success;
    public interface IIERXmlRpcRaw
    {
        Either<IERApiError, Success> SetAuthorisation(int nbpassage, int direction);
        //Option<object[]> ResetAuthorisation(int[] param);
        Either<IERApiError, Success> SetEmergency(bool bEnabled);
        Either<IERApiError, Success> SetMaintenanceMode(bool bEnabled);
        Either<IERApiError, Success> Reboot();
        Either<IERApiError, Success> Restart();
        Option<object[]> ApplyUpdate();
        Either<IERApiError, Success> SetDate(DateTime dt, string timezone = "");
        Either<IERApiError, DateTime> GetDate();
        Option<object> GetStatusFull();
        Either<IERApiError, IERStatus> GetStatus();
        Either<IERApiError, IERSWVersion> GetVersion();
        Option<object[]> GetCounter();
        Option<object[]> SetMode(string[] param);
        Option<object[]> SetCredentials(string[] param);
        Either<IERApiError, Success> SetTempo(TempoConf conf);
        Either<IERApiError, TempoConf> GetTempo();
        Option<object[]> GetSetTempoFlow(int[] param);
        Option<object[]> GetCurrentPassage();
        Option<object[]> SetOutputClient(int[] param);
        Option<object[]> GetMotorSpeed();
        Option<object[]> SetMotorSpeed(object[] param);
        Either<IERApiError, Success> SetBuzzerFraud(int volume, int note);
        Either<IERApiError, Success> SetBuzzerIntrusion(int volume, int note);
        // SetBuzzerIntrusion is not mentioned in the document. Imlies that it must be old        
        Option<object[]> SetBuzzerMode(int[] param);
    }
}
