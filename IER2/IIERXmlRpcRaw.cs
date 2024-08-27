using LanguageExt;

namespace EtGate.IER
{
    public interface IIERXmlRpcRaw
    {
        Option<object[]> SetAuthorisation(int[] param);
        Option<object[]> SetEmergency(int[] param);
        Option<object[]> SetMaintenanceMode(int[] param);
        Option<object[]> Reboot();
        Option<object[]> Restart();
        Option<object[]> ApplyUpdate();
        Option<object[]> SetDate(object[] param);
        Option<object[]> GetDate();
        Option<object> GetStatusEx();
        Option<object[]> GetVersion();
        Option<object[]> GetCounter();
        Option<object[]> SetMode(string[] param);
        Option<object[]> SetCredentials(string[] param);
        Option<object[]> GetSetTempo(int[] param);
        Option<object[]> GetSetTempoFlow(int[] param);
        Option<object[]> GetCurrentPassage();
        Option<object[]> SetOutputClient(int[] param);
        Option<object[]> GetMotorSpeed();
        Option<object[]> SetMotorSpeed(object[] param);
        Option<object[]> SetBuzzerFraud(int[] param);
        Option<object[]> SetBuzzerIntrusion(int[] param);
        Option<object[]> SetBuzzerMode(int[] param);
        Option<IERStatus> GetStatus();
    }
}
