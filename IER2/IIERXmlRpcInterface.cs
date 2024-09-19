using Horizon.XmlRpc.Client;
using Horizon.XmlRpc.Core;

namespace EtGate.IER
{

    public interface IIERXmlRpcInterface : IXmlRpcProxy
    {
        [XmlRpcMethod("SendReboot")]//endpoint name
        object[] SendReboot();

        [XmlRpcMethod("SendRestart")]//endpoint name
        object[] SendRestart();

        [XmlRpcMethod("ApplyUpdate")]//endpoint name
        object[] ApplyUpdate();

        [XmlRpcMethod("GetBuzzerFraud")]//endpoint name
        object[] GetBuzzerFraud();

        [XmlRpcMethod("GetBuzzerIntrusion")]//endpoint name
        object[] GetBuzzerIntrusion();

        [XmlRpcMethod("GetClientInfo")]//endpoint name
        object[] GetClientInfo();

        [XmlRpcMethod("GetDate")]//endpoint name
        object[] GetDate();

        //GetDoorOperationModeList
        [XmlRpcMethod("GetDoorOperationModeList")]//endpoint name
        object[] GetDoorOperationModeList();

        //GetInputs
        [XmlRpcMethod("GetInputs")]//endpoint name
        object[] GetInputs();



        //GetStatus
        [XmlRpcMethod("GetStatus")]//endpoint name
        object[] GetStatus();

        //GetStatus
        [XmlRpcMethod("GetStatusStd")]//endpoint name
        object GetStatusStd();

        //GetVersion
        [XmlRpcMethod("GetVersion")]//endpoint name
        object[] GetVersion();

        //GetCurrentPassage  Return the summary of the last passage.
        [XmlRpcMethod("GetCurrentPassage")]//endpoint name
        object[] GetCurrentPassage();

        //GetCounter  Get current values of counter
        [XmlRpcMethod("GetCounter")]//endpoint name
        object[] GetCounter();
        //Set 
        //SetAuthorisation

        [XmlRpcMethod("SetAuthorisation")]//endpoint name
        object[] SetAuthorisation(params int[] par);

        //SetBuzzerFraud
        [XmlRpcMethod("SetBuzzerFraud")]//endpoint name
        object[] SetBuzzerFraud(params int[] par);

        //SetBuzzerIntrusion
        [XmlRpcMethod("SetBuzzerIntrusion")]//endpoint name
        object[] SetBuzzerIntrusion(params int[] par);

        //SetBuzzerMode
        [XmlRpcMethod("SetBuzzerMode")]//endpoint name
        object[] SetBuzzerMode(params int[] par);

        //SetEmergency
        /*
         * input = 0  type int Emergency
            mode.
            0: Disiabled
            1: Enabled
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [XmlRpcMethod("SetEmergency")]//endpoint name
        object[] SetEmergency(params int[] mod);

        //SetMaintenanceMode
        [XmlRpcMethod("SetMaintenanceMode")]//endpoint name
        object[] SetMaintenanceMode(params int[] mod);

        //SetMode
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gatemode"> [0] -> Door Modes , [1] -> Entry sSide Operating Mode , [2] Exit side operating mode</param>
        /// <returns></returns>
        [XmlRpcMethod("SetMode")]//endpoint name
        object[] SetMode(params string[] gatemode);

        //SetCredentials
        [XmlRpcMethod("SetCredentials")]//endpoint name
        object[] SetCredentials(params string[] creds);

        //SetDate
        [XmlRpcMethod("SetDate")]//endpoint name
        object[] SetDate(params object[] dt);

        /// <summary>
        /// GetMotorSpeed, gets the motor opening & closing speed 
        /// </summary>
        /// <returns></returns>
        [XmlRpcMethod("GetMotorSpeed")]//endpoint name
        object[] GetMotorSpeed();

        //SetMotorSpeed
        [XmlRpcMethod("SetMotorSpeed")]//endpoint name
        object[] SetMotorSpeed(params object[] param);

        /// <summary>
        /// ange the timer durations (ms) and return the current/new values in Results.
        /// </summary>
        /// <returns></returns>
        [XmlRpcMethod("GetSetTempo")]//endpoint name
        object[] GetSetTempo(params int[] param);


        /// <summary>
        /// Change the timer durations (ms) and return the current/new values in Results..
        /// </summary>
        /// <returns></returns>
        [XmlRpcMethod("GetSetTempoFlow")]//endpoint name
        object[] GetSetTempoFlow(params int[] param);



        [XmlRpcMethod("system.methodHelp")]//endpoint name
        string methodHelp(string MethodName);

        [XmlRpcMethod("SetOutputClient")]
        object[] SetOutputClient(params int[] param);
    }
}
