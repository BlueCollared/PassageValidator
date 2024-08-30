using IFS2.Equipment.DriverInterface;
using IFS2.Equipment.HardwareInterface.IERPLCManager;
using LanguageExt;

using RetTypSetFn = LanguageExt.Either<EtGate.IER.IERApiError, EtGate.IER.Success>;

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
    public interface IIERXmlRpc
    {
        RetTypSetFn SetAuthorisation(int nbpassage, int direction);
        //Option<object[]> ResetAuthorisation(int[] param);
        RetTypSetFn SetEmergency(bool bEnabled);
        RetTypSetFn SetMaintenanceMode(bool bEnabled);
        RetTypSetFn Reboot();
        RetTypSetFn Restart();
        Option<object[]> ApplyUpdate();
        RetTypSetFn SetDate(DateTime dt, string timezone = "");
        Either<IERApiError, DateTime> GetDate();
        Option<object> GetStatusFull();
        Either<IERApiError, IERStatus> GetStatus();
        Either<IERApiError, IERSWVersion> GetVersion();
        Option<object[]> GetCounter();
        RetTypSetFn SetMode(Option<DoorsMode> doorsMode, Option<SideOperatingModes> entry, Option<SideOperatingModes> exit);
        Option<object[]> SetCredentials(string[] param);
        RetTypSetFn SetTempo(TempoConf conf);
        Either<IERApiError, TempoConf> GetTempo();
        Option<object[]> GetSetTempoFlow(int[] param);
        Option<object[]> GetCurrentPassage();
        Option<object[]> SetOutputClient(int[] param);
        Option<object[]> GetMotorSpeed();
        Option<object[]> SetMotorSpeed(object[] param);
        RetTypSetFn SetBuzzerFraud(int volume, int note);
        RetTypSetFn SetBuzzerIntrusion(int volume, int note);
        // SetBuzzerIntrusion is not mentioned in the document. Imlies that it must be old        
        Option<object[]> SetBuzzerMode(int[] param);
    }
}
