namespace EtGate.IER
{
    public interface IIERXmlRpcRaw
    {
        object[] SetAuthorization(int[] param);
        object[] SetEmergency(int[] param);
        object[] SetMaintenanceMode(int[] param);
        object[] Reboot();
        object[] Restart();
        object[] ApplyUpdate();
        object[] SetDate(object[] param);
        object[] GetDate();
        object GetStatusEx();
        object[] GetVersion();
        object[] GetCounter();
        object[] SetMode(string[] param);
        object[] SetCredentials(string[] param);
        object[] GetSetTempo(int[] param);
        object[] GetSetTempoFlow(int[] param);
        object[] GetCurrentPassage();
        object[] SetOutputClient(int[] param);
        object[] GetMotorSpeed();
        object[] SetMotorSpeed(object[] param);
        object[] SetBuzzerFraud(int[] param);
        object[] SetBuzzerIntrusion(int[] param);
        object[] SetBuzzerMode(int[] param);
    }
}
