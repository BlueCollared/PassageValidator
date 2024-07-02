using IFS2.Equipment.DriverInterface;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager
{
    public enum SecurityProtocol
    {
        None,
        IsSSH,
        IsTls,
        IsSSHKeyFile
    }
    public enum FileTransfertMode
    {
        None,
        Simulation,
        Ftp,
        Sftp,
        SftpWinscp,
        Ftps
    }
    public enum PLC_IER_ConfigurationKeys
    {
        /// <summary>
        /// 
        /// </summary>
        IPAddr,
        /// <summary>
        /// 
        /// </summary>
        PortNum,
        /// <summary>
        /// 
        /// </summary>
        ReadTimeout,//ms

        MotorSpeed,

        DoorOpenTime,//ms

        DoorCloseTime,//ms

        SSLCertificatePath,

        LoginId,

        Passowrd,
        TimeZone
    }

    [XmlRoot("Data")]
    public class FileTransfertConfiguration
    {
        [XmlElement("Mode")]
        public string _ModeStr = "None";

        [XmlIgnore]
        public bool _Mode_Initialised = false;

        [XmlIgnore]
        public FileTransfertMode _Mode = FileTransfertMode.None;

        [XmlElement("ServerUrl")]
        public string ServerUrl = "127.0.0.1";

        [XmlElement("PortNumber")]
        public int PortNumber = 21;

        [XmlElement("Root")]
        public string Root = "";

        [XmlElement("TimeoutConnection")]
        public int TimeoutConnection = 20000;

        [XmlElement("TimeoutPutFile")]
        public int TimeoutPutFile = 30000;

        [XmlElement("TimeoutGetFile")]
        public int TimeoutGetFile = 30000;

        [XmlElement("TimeoutDeleteFile")]
        public int TimeoutDeleteFile = 20000;

        [XmlElement("TimeoutListFiles")]
        public int TimeoutListFiles = 30000;

        [XmlElement("Login")]
        public string Login = "user";

        [XmlElement("Password")]
        public string Password = "";

        [XmlElement("SshHostKeyFingerprint")]
        public string SshHostKeyFingerprint = "";

        [XmlElement("LogPath")]
        public string LogPath = "";//DiskUtilities.BaseLogDirectory;

        [XmlElement("LogFile")]
        public string LogFile = "";

        [XmlElement("PrivateKeyFile")]
        public string PrivateKeyFile = "my-ssh-key.ppk";

        [XmlElement("PublicKeyFile")]
        public string PublicKeyFile = "my-ssh-key.pub";

        [XmlElement("PassPhrase")]
        public string PassPhrase = "";

        [XmlElement("SecMode")]
        public string _SecModeStr = "None";

        [XmlIgnore]
        public bool _SecMode_Initialised = false;

        [XmlIgnore]
        public SecurityProtocol _SecMode = SecurityProtocol.None;

        [XmlElement("PassiveMode")]
        public bool PassiveMode = true;

        [XmlElement("Certificate")]
        public string Certificate = "";

        [XmlIgnore]
        public FileTransfertMode Mode
        {
            get
            {
                try
                {
                    if (_Mode_Initialised)
                    {
                        return _Mode;
                    }

                    _Mode = EnumUtilities.ParseEnum<FileTransfertMode>(_ModeStr);
                    _Mode_Initialised = true;
                    return _Mode;
                }
                catch
                {
                    _Mode = FileTransfertMode.None;
                    _Mode_Initialised = true;
                    return FileTransfertMode.None;
                }
            }
            set
            {
                _Mode = value;
                _Mode_Initialised = true;
                _ModeStr = _Mode.ToString();
            }
        }

        [XmlIgnore]
        public SecurityProtocol SecMode
        {
            get
            {
                try
                {
                    if (_SecMode_Initialised)
                    {
                        return _SecMode;
                    }

                    _SecMode = EnumUtilities.ParseEnum<SecurityProtocol>(_SecModeStr);
                    _SecMode_Initialised = true;
                    return _SecMode;
                }
                catch
                {
                    _SecMode = SecurityProtocol.None;
                    _SecMode_Initialised = true;
                    return SecurityProtocol.None;
                }
            }
            set
            {
                _SecMode = value;
                _SecMode_Initialised = true;
                _SecModeStr = _SecMode.ToString();
            }
        }

        public string ToXMLString()
        {
            throw new NotImplementedException();
        }

        public static FileTransfertConfiguration FromXMLString(string data)
        {
            throw new NotImplementedException();
        }
    }

    internal class EnumUtilities
    {
        internal static T ParseEnum<T>(string modeStr)
        {
            throw new NotImplementedException();
        }
    }

    [XmlRoot("Data")]
    public class IERPLCConfigurationData
    {
        [XmlElement("FlapRemainOpenPostFreePasses")]
        [JsonInclude]
        [JsonPropertyName("FlapRemainOpenPostFreePasses")]
        public int FlapRemainOpenPostFreePasses = 2000;

        [XmlElement("FlapRemainOpenPostControlledPasses")]
        [JsonInclude]
        [JsonPropertyName("FlapRemainOpenPostControlledPasses")]
        public int FlapRemainOpenPostControlledPasses = 2000;

        [XmlElement("TimeToEnterAfterAuthorisation")]
        [JsonInclude]
        [JsonPropertyName("TimeToEnterAfterAuthorisation")]
        public int TimeToEnterAfterAuthorisation = 2000;

        [XmlElement("TimeToValidateAfterDetection")]
        [JsonInclude]
        [JsonPropertyName("TimeToValidateAfterDetection")]
        public int TimeToValidateAfterDetection = 4000;

        [XmlElement("TimeToCrossAfterDetection")]
        [JsonInclude]
        [JsonPropertyName("TimeToCrossAfterDetection")]
        public int TimeToCrossAfterDetection = 10000;

        [XmlElement("TimeAllowedToExitSafetyZone")]
        [JsonInclude]
        [JsonPropertyName("TimeAllowedToExitSafetyZone")]
        public int TimeAllowedToExitSafetyZone = 5000;

        [XmlElement("TimeAllowedToCrossLaneAfterAuthorisation")]
        [JsonInclude]
        [JsonPropertyName("TimeAllowedToCrossLaneAfterAuthorisation")]
        public int TimeAllowedToCrossLaneAfterAuthorisation = 10000;

        [XmlElement("PassageClearTimeout")]
        [JsonInclude]
        [JsonPropertyName("PassageClearTimeout")]
        public int PassageClearTimeout = 12000;

        [XmlElement("StandardOpeningSpeed")]
        [JsonInclude]
        [JsonPropertyName("StandardOpeningSpeed")]
        public int StandardOpeningSpeed = 100;

        [XmlElement("StandardClosingSpeed")]
        [JsonInclude]
        [JsonPropertyName("StandardClosingSpeed")]
        public int StandardClosingSpeed = 100;

        [XmlElement("SecurityOpeningSpeed")]
        [JsonInclude]
        [JsonPropertyName("SecurityOpeningSpeed")]
        public int SecurityOpeningSpeed = 20;

        [XmlElement("DisappearanceSpeed")]
        [JsonInclude]
        [JsonPropertyName("DisappearanceSpeed")]
        public int DisappearanceSpeed = 20;

        [XmlElement("FraudClosingSpeed")]
        [JsonInclude]
        [JsonPropertyName("FraudClosingSpeed")]
        public int FraudClosingSpeed = 100;

        [XmlElement("SecurityFraudClosingSpeed")]
        [JsonInclude]
        [JsonPropertyName("SecurityFraudClosingSpeed")]
        public int SecurityFraudClosingSpeed = 20;

        [XmlElement("AutomaticChangeModeAtNight")]
        [JsonInclude]
        [JsonPropertyName("AutomaticChangeModeAtNight")]
        public bool AutomaticChangeModeAtNight = false;

        [XmlElement("AutomaticTimeForInService")]
        [JsonInclude]
        [JsonPropertyName("AutomaticTimeForInService")]
        public string _AutomaticTimeForInService = "02:30:00";

        [XmlElement("AutomaticTimeForOutOfService")]
        [JsonInclude]
        [JsonPropertyName("AutomaticTimeForOutOfService")]
        public string _AutomaticTimeForOutOfService = "04:30:00";

        [XmlIgnore]
        [JsonIgnore]
        public DateTime AutomaticTimeForInService
        {
            get
            {
                return Convert.ToDateTime(_AutomaticTimeForInService);
            }
            set
            {
                _AutomaticTimeForInService = value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public DateTime AutomaticTimeForOutOfService
        {
            get
            {
                return Convert.ToDateTime(_AutomaticTimeForOutOfService);
            }
            set
            {
                _AutomaticTimeForOutOfService = value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public IERPLCConfigurationData(IERPLCConfigurationData init)
        {
            FlapRemainOpenPostFreePasses = init.FlapRemainOpenPostFreePasses;
            FlapRemainOpenPostControlledPasses = init.FlapRemainOpenPostControlledPasses;
            TimeToEnterAfterAuthorisation = init.TimeToEnterAfterAuthorisation;
            TimeToValidateAfterDetection = init.TimeToValidateAfterDetection;
            TimeToCrossAfterDetection = init.TimeToCrossAfterDetection;
            TimeAllowedToExitSafetyZone = init.TimeAllowedToExitSafetyZone;
            TimeAllowedToCrossLaneAfterAuthorisation = init.TimeAllowedToCrossLaneAfterAuthorisation;
            PassageClearTimeout = init.PassageClearTimeout;
            StandardOpeningSpeed = init.StandardOpeningSpeed;
            StandardClosingSpeed = init.StandardClosingSpeed;
            SecurityOpeningSpeed = init.SecurityOpeningSpeed;
            DisappearanceSpeed = init.DisappearanceSpeed;
            FraudClosingSpeed = init.FraudClosingSpeed;
            SecurityFraudClosingSpeed = init.SecurityFraudClosingSpeed;
            AutomaticChangeModeAtNight = init.AutomaticChangeModeAtNight;
            AutomaticTimeForInService = init.AutomaticTimeForInService;
            AutomaticTimeForOutOfService = init.AutomaticTimeForOutOfService;
        }

        public IERPLCConfigurationData()
        {
        }

        public string ToXMLString()
        {
            throw new NotImplementedException();
        }

        public string ToJSONString()
        {
            throw new NotImplementedException();
        }

        public static IERPLCConfigurationData FromXMLString(string data)
        {
            throw new NotImplementedException();
        }

        public static IERPLCConfigurationData FromJSONString(string data)
        {
            throw new NotImplementedException();
        }
    }

    public static class LocalConfiguration
    {
        internal static string ReadStringParameter(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        internal static int ReadIntParameter(string v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
    public static class Logging
    {
        internal static void Information(object logContext, string v)
        {
            throw new NotImplementedException();
        }

        internal static void Verbose(object logContext, string v)
        {
            throw new NotImplementedException();
        }

        internal static void Error(object logContext, string v)
        {
            throw new NotImplementedException();
        }
    }
    public class CIERPLCMain //: PLCHardwareInterfaceModel, IPLCHardwareInterface
    {

        /// <summary>
        /// Used for logging in others classes
        /// </summary>
        //public static LogContext logContext = null;

        private CPLCStatus _plcstatus;
        /// <summary>
        /// Boolean to indicate if hardware is connected.
        /// </summary>
        private bool _isDeviceConnected = false;
        bool PasageError = false;
        private string _ipAddress = "";
        private int _portNum = 8888;
        private string _timeZone = "Europe/Paris";
        int _NoOfAuthorization = 0;
        int _gateOldStatus = 0;
        private CPLCStatus _old_plcstatus;
        private object LogContext;


        public CIERPLCMain()
        {
            _plcstatus = new CPLCStatus();
        }
        /// <summary>
        /// Initialisation of Hardware Driver
        /// </summary>
        /// <param name="driverName"></param>
        /// <param name="peripheralName"></param>
        /// <param name="generalParameters"></param>
        /// <returns>0 if OK. -1 if error during Initialisation process</returns>
        public int Initialise(string driverName, string peripheralName
            //, DriverManagerParameterFormat generalParameters, LogContext logContext_
            )
        {
            try
            {
                //CIERRpcHelper.InitializeRpc();
                //if (base.Initialise(driverName, peripheralName, generalParameters, logContext_) < 0) return -1; // error when initialising base class.                                                                      //http://192.168.0.200
                _ipAddress = LocalConfiguration.ReadStringParameter(PLC_IER_ConfigurationKeys.IPAddr.ToString(), "http://192.168.0.200");
                int.TryParse(LocalConfiguration.ReadStringParameter(PLC_IER_ConfigurationKeys.PortNum.ToString(), "8081"), out _portNum);
                _timeZone = LocalConfiguration.ReadStringParameter(PLC_IER_ConfigurationKeys.TimeZone.ToString(), "Europe/Paris");//timezone as per IER document

                object LogContext = null;
                Logging.Information(LogContext, "CIERPLCMain.Initialise.Created");
                _isDeviceConnected = false;
                if (CIERRpcHelper.InitializeRpc(_ipAddress, _portNum.ToString()))
                {
                    Logging.Information(LogContext, "CIERPLCMain.Initialise.Connection.Success ");
                    //string[] str = CIERRpcHelper.ListAvailableMethods();
                    //Console.WriteLine("List of Method");
                    //foreach (var item in str)
                    //{
                    //    Console.WriteLine("" + item);
                    //}
                    _isDeviceConnected = true;
                }
                else
                {
                    Logging.Error(LogContext, "CIERPLCMain.Initialise.Connection.Failed ");
                }
                return (0);
            }
            catch (System.Net.WebException ex1)
            {
                _isDeviceConnected = false;
                Logging.Information(LogContext, "CIERPLCMain.Initialise.Creation.Failed witt WebException: " + ex1.Message + " ipAddress:" + _ipAddress + " portNum:" + _portNum);
            }
            catch (Exception e)
            {
                _isDeviceConnected = false;
                object LogContext = null;
                Logging.Information(LogContext, "CIERPLCMain.Initialise.Creation.Failed " + e.Message + " ipAddress:" + _ipAddress + " portNum:" + _portNum);
            }
            return -1;
        }



        /// <summary>
        /// Reboot
        /// </summary>
        /// <param name="bHardboot"></param>
        /// <returns></returns>
        public STATUS Reboot(bool bHardboot)
        {
            Logging.Verbose(LogContext, "CIERPLCMain.Reboot.In PLCConnected: " + _isDeviceConnected.ToString() + " Hardboot:" + bHardboot.ToString());
            try
            {
                if (_isDeviceConnected)
                {
                    object[] result = CIERRpcHelper.Reboot(bHardboot);
                    if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
                }
            }
            catch (System.Net.WebException ex)
            {
                _isDeviceConnected = false;
                Logging.Error(LogContext, "CIERPLCMain.Reboot.out Exception:" + ex.Message);
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "CIERPLCMain.Reboot.out Exception:" + e.Message);
            }
            Logging.Verbose(LogContext, "CIERPLCMain.Reboot.out");
            return STATUS.NOK;
        }

        

        /// <summary>
        /// GetFirmwareVersion
        /// </summary>
        /// <param name="firmwareVersion"></param>
        /// <returns></returns>
        public bool GetFirmwareVersion(out string firmwareVersion)
        {
            firmwareVersion = "X.0.XXX";

            try
            {
                IERSWVersion verinfo = CIERRpcHelper.GetVersion();
                firmwareVersion = verinfo.SWVersion;
                return true;
            }
            catch (System.Net.WebException ex)
            {
                _isDeviceConnected = false;
                Logging.Error(LogContext, "CIERPLCMain.GetFirmwareVersion WebException:" + ex.Message);
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "CIERPLCMain.GetFirmwareVersion Exception:" + e.Message);
            }
            return false;
        }

        /// <summary>
        /// GetStatus
        /// </summary>
        /// <returns></returns>
        public CPLCStatus GetStatus()
        {
            _plcstatus = new CPLCStatus();
            _plcstatus.Reset();
            try
            {
                object result = CIERRpcHelper.GetStatusEx();
                GetStatusStdAnswer getStatusStdAnswer = new GetStatusStdAnswer();
                //Decode buffer for Log
                //getStatusStdAnswer.Set(result,LogContext);
                _plcstatus.rawData = new Dictionary<string, object>();
                int val = CIERRpcHelper.ResponseParser(result, ref _plcstatus.rawData);
                if (val > 0)
                {
                    _plcstatus.callreturnStatus = STATUS.OK;
                    DecodeStatusData(ref _plcstatus,getStatusStdAnswer);                    
                }
                else
                {
                    _plcstatus.callreturnStatus = STATUS.NOK;
                }
            }
            catch (System.Net.Sockets.SocketException tx)
            {
                _plcstatus.callreturnStatus = STATUS.GCU_COMM_ERROR;
            }
            catch (System.Net.Http.HttpRequestException rx)
            {
                _plcstatus.callreturnStatus = STATUS.GCU_COMM_ERROR;
            }
            catch (System.Net.WebException ex1)
            {
                //_plcstatus.callreturnStatus = STATUS.NOK;
                _plcstatus.callreturnStatus = STATUS.GCU_COMM_ERROR;
            }
            catch (Exception ex)
            {
                _plcstatus.callreturnStatus = STATUS.NOK;
            }
            return _plcstatus;
        }

        public enum EventAlarmLevel
        {
            /// <summary>
            /// Value when level of alarm is unknwn, generally initialisation is missing in this case
            /// </summary>
            Unknown = 99,
            /// <summary>
            /// Status of event is normal. When event is an alarm it means that there is no alarm.
            /// </summary>
            Normal = 0,
            /// <summary>
            /// There is a warning but element is not out of service and can continue to work.
            /// </summary>
            Warning = 2,
            /// <summary>
            /// There is an alarm and element is out of service.
            /// </summary>
            Alarm = 3,
            /// <summary>
            /// Concerned element is not conected
            /// </summary>
            NotConnected = 98
        }


        private void AnalyseFailureList(List<string> list,EventAlarmLevel level, CPLCStatus status)
        {


            foreach (string failure in list)
            {
                switch (failure.ToUpper())
                {
                    case "ERRORS_CAMERA_HEIGHT":
                    case "ERRORS_CAMERA_NG":
                    case "ERRORS_CAMERA_VERSION":
                    case "ERRORS_FRONTAL_DETECTION":
                    case "ERRORS_FRONTAL_OCCULTED":
                        //status.Camera = level;
                        status.CameraDetail+= (status.CameraDetail!=""? ";":"") + failure;
                        break;
                    case "ERRORS_CAN":
                    case "ERRORS_CAN_HEARTBEAT":
                    case "ERRORS_CAN_OVERFLOW":
                    case "ERRORS_CAN_PRODUCT_CODE":
                    case "ERRORS_CAN_SW_VERSION":
                        //status.CanBus = level;
                        status.CanBusDetail += (status.CanBusDetail != "" ? ";" : "") + failure;
                        break;
                    case "ERRORS_CELL_IR":
                    case "ERRORS_DETECTION":
                    case "ERRORS_DIRAS_EMITTER":
                    case "ERRORS_DIRAS_RECEIVER":
                    case "ERRORS_OBSTRUCTED_CELLS":
                    case "ERRORS_LATERAL_DETECTION":
                        //status.Sensors = level;
                        status.SensorsDetail += (status.SensorsDetail != "" ? ";" : "") + failure;
                        break;
                    case "ERRORS_BLOCKED_MOTOR":
                    case "ERRORS_MOTOR_CARD":
                    case "ERRORS_MOTOR_CARD_CONFIG":
                    case "ERRORS_MOTOR_INIT":
                    case "ERRORS_MOTOR_PEAK_CURRENT":
                    case "ERRORS_HALL_EFFECT_FAILURE":
                        //status.Motor = level;
                        status.MotorDetail += (status.MotorDetail != "" ? ";" : "") + failure;
                        break;
                    case "ERRORS_MOTOR_BRAKE":
                    case "ERRORS_MOTOR_BRAKE_FAILURE":
                        //status.Brake = level;
                        status.BrakeDetail += (status.BrakeDetail != "" ? ";" : "") + failure;
                        break;
                    case "ERRORS_INSTALLATION":
                    case "ERRORS_CPU":
                    case "ERRORS_EXT":
                        //status.GCUElectronic = level;
                        status.GCUElectronicDetail += (status.GCUElectronicDetail != "" ? ";" : "") + failure;
                        break;
                    case "ERRORS_SYSTEMD_FAILURE":
                    case "ERRORS_USINE_TEST_WARNING":
                        //status.GCUSystem = level;
                        status.GCUSystemDetail += (status.GCUSystemDetail != "" ? ";" : "") + failure;
                        break;
                    case "ERRORS_CUSTOMER":
                    case "ERRORS_CUSTOMER_FAILURE":
                        //status.AFCCommunication = level;
                        status.AFCCommunicationDetail += (status.AFCCommunicationDetail != "" ? ";" : "") + failure;
                        break;
                    case "ERRORS_TEMPERATURE":
                    case "ERRORS_THERMOPILE":
                        //status.GCUTemperature = level;
                        status.GCUTemperatureDetail += (status.GCUTemperatureDetail != "" ? ";" : "") + failure;
                        break;
                    //Following are not used
                    case "ERRORS_EGRESS":
                    case "ERRORS_OPENED_SERVICE_DOOR":
                    case "ERRORS_FTP":
                    case "ERRORS_MODBUS_SERIAL_FAILURE":
                    case "ERRORS_MODBUS_TCP_FAILURE":
                    case "ERRORS_LCD":
                    case "ERRORS_READER":
                    case "ERRORS_SOUNDPLAYER":
                        break;
                }
            }
        }

        private void AnalyseAlarmsList(GetStatusStdAnswer status,  CPLCStatus _plcstatus)
        {
            _plcstatus.bWWIntrusionExit = false;
            _plcstatus.bWWIntrusionEntry = false;
            _plcstatus.bFraudEntry = false;
            _plcstatus.bFraudExit = false;
            _plcstatus.bFraud = false;
            _plcstatus.bFraudRamp = false;
            _plcstatus.bFraudOthers = false;
            _plcstatus.bWWIntrusion = false;
            _plcstatus.bPreIntrusionEntry = false;
            _plcstatus.bPreIntrusionExit = false;
            _plcstatus.bPreIntrusion = false;

            if (status.alarms.Count > 0)
            {
                foreach (string str in status.alarms)
                {
                    switch (str)
                    {
                        case "LANG_FRAUD_A":
                            //A person has cross the entry without authorisation. => need to display to go back.
                            //On next side prohibited if authorised. If one validation result let it.
                            _plcstatus.bFraudEntry = true;
                            break;
                        case "LANG_FRAUD_B":
                            //A person has cross the exit without authorisation
                            _plcstatus.bFraudExit = true;
                            break;
                        case "LANG_FRAUD_HOLDING":
                        case "LANG_FRAUD_MANTRAP":
                        case "LANG_FRAUD_UNEXPECTED_MOTION":
                            _plcstatus.bFraudOthers = true;
                            break;
                        case "LANG_FRAUD_JUMP":
                            _plcstatus.bFraudJump = true;
                            break;
                        case "LANG_FRAUD_RAMPING":
                            _plcstatus.bFraudRamp = true;
                            break;
                        case "LANG_INTRUSION_A":
                        case "LANG_OPPOSITE_INTRUSION_A":
                            //A person stays in the entry. => ask to exit. next side shall be prohibited
                            _plcstatus.bWWIntrusionEntry = true;
                            break;
                        case "LANG_INTRUSION_B":
                        case "LANG_OPPOSITE_INTRUSION_B":
                            _plcstatus.bWWIntrusionExit = true;
                            break;
                        case "LANG_PREALARM_A":
                            _plcstatus.bPreIntrusionEntry = true;
                            break;
                        case "LANG_PREALARM_B":
                            _plcstatus.bPreIntrusionExit = true;
                            break;
                        case "LANG_FRAUD_DISAPPEARANCE": 
                            break;
                    }
                }
            }
            _plcstatus.bFraud = _plcstatus.bFraudJump || _plcstatus.bFraudRamp || _plcstatus.bFraudExit || _plcstatus.bFraudEntry||_plcstatus.bFraudOthers;
            _plcstatus.bWWIntrusion = _plcstatus.bWWIntrusionEntry || _plcstatus.bWWIntrusionExit;
            _plcstatus.bPreIntrusion = _plcstatus.bPreIntrusionEntry || _plcstatus.bPreIntrusionExit;
        }


        private void TreatInputPart(GetStatusStdAnswer status, CPLCStatus cPLCStatus)
        {
            cPLCStatus.bPoweredOnUPS = false;
            cPLCStatus.bEntryMaintenanceSwitch = false;
            cPLCStatus.bExitMaintenanceSwitch = false;
            cPLCStatus.bEmergencyButton = false;
            DictionaryXmlInt dico = status.inputs.Find(x => x.key == "emergency");
            if ((dico != null) &&(dico.value==1)) cPLCStatus.bEmergencyButton=true;
            dico = status.inputs.Find(x => x.key == "entry_locked_open");
            if ((dico != null) && (dico.value == 1)) cPLCStatus.bEntryMaintenanceSwitch = true; 
            dico = status.inputs.Find(x => x.key == "ups");
            if ((dico != null) && (dico.value == 1)) cPLCStatus.bPoweredOnUPS = true;
        }

        private void TreatFailuresPart(GetStatusStdAnswer status, CPLCStatus cPLCStatus)
        {
            //cPLCStatus.Camera = EventAlarmLevel.Normal;
            //cPLCStatus.CameraDetail = "";
            //cPLCStatus.CanBus = EventAlarmLevel.Normal;
            //cPLCStatus.CanBusDetail = "";
            //cPLCStatus.Sensors = EventAlarmLevel.Normal;
            //cPLCStatus.SensorsDetail = ""; 
            //cPLCStatus.Motor = EventAlarmLevel.Normal;
            //cPLCStatus.MotorDetail = ""; 
            //cPLCStatus.Brake = EventAlarmLevel.Normal;
            //cPLCStatus.BrakeDetail = ""; 
            //cPLCStatus.GCUElectronic = EventAlarmLevel.Normal;
            //cPLCStatus.GCUElectronicDetail = ""; 
            //cPLCStatus.GCUSystem = EventAlarmLevel.Normal;
            //cPLCStatus.GCUSystemDetail = "";
            //cPLCStatus.AFCCommunication = EventAlarmLevel.Normal;
            //cPLCStatus.AFCCommunicationDetail = "";
            //cPLCStatus.GCUTemperature = EventAlarmLevel.Normal;
            cPLCStatus.GCUTemperatureDetail = "";
            if (status.failures.Count > 0)
            {
                ForFailure failures = status.failures.Find(x => x.key == "minor");
                if (failures != null)
                {
                    AnalyseFailureList(failures.list,EventAlarmLevel.Warning,cPLCStatus);
                }
                failures= status.failures.Find(x => x.key == "major");
                if (failures != null)
                {
                    AnalyseFailureList(failures.list, EventAlarmLevel.Alarm, cPLCStatus);
                }
            }
        }


        /// <summary>
        /// DecodeStatusData
        /// </summary>
        /// <param name="cPLCStatus"></param>
        private void DecodeStatusData(ref CPLCStatus cPLCStatus, GetStatusStdAnswer status)
        {
            cPLCStatus.Clear();
            PasageError = false;
            //Number of authorisations in direction A (in entry)
            int AuthorizationA = (int)cPLCStatus.rawData["AuthorizationA"];
            //Number of authorisations in direction B (exit)
            int AuthorizationB = (int)cPLCStatus.rawData["AuthorizationB"];
            //Current movement of obstacle.
            //'DOOR_CLOSED' string The door is closed
            //'DOOR_CLOSING_FROM_A'string The door is currently closing from the A side
            //'DOOR_CLOSING_FROM_B'string The door is currently closing from the B side
            //'DOOR_OPENING_A' string The door is opening towards the A side
            //'DOOR_OPENING_B' string The door is opening towards the B side
            //'DOOR_OPEN_A' string The door is open towards the A side
            //'DOOR_OPEN_B' string The door is open towards the B side
            //TOSEE what is usage because seems not used ?
            string Doors = (string)((IDictionary)cPLCStatus.rawData["doors"])["state"];
            cPLCStatus.Authorizations[0] = AuthorizationA;
            cPLCStatus.Authorizations[1] = AuthorizationB;
            //Indication of error on obstable 0 or 1
            int val = (int)((IDictionary)cPLCStatus.rawData["doors"])["error"];
            if (val == 0) cPLCStatus.bDoorFailure = false;
            else cPLCStatus.bDoorFailure = true;
            //Initialized --skipping at the moment  //Door is initialised (1-0)
            //int val=(int) ((IDictionary)cPLCStatus.rawData["doors"])["initialized"];
            //TOSEE could we have authorisations in the 2 sides.
            //_NoOfAuthorization = 0; //JL : Added Initialisation of value
            //if (AuthorizationA > 0)
            //    _NoOfAuthorization = AuthorizationA;
            //else if (AuthorizationB > 0)
            //    _NoOfAuthorization = AuthorizationB;
            _NoOfAuthorization = AuthorizationA > 0 ? AuthorizationA : (AuthorizationB > 0 ? AuthorizationA : 0);
            //bSafetyZoneActivated
            val = (int)((IDictionary)cPLCStatus.rawData["doors"])["safety_zone"];
            if (val == 0) cPLCStatus.bSafetyZoneActivated = false;
            else cPLCStatus.bSafetyZoneActivated = true;
            //TOSEE Not clear in IER doc usage of 2 following. But at the end somebody is in safety zone.
            //safety_zoneA (entry side)
            val = (int)((IDictionary)cPLCStatus.rawData["doors"])["safety_zone_A"];
            cPLCStatus.bSafetyZoneActivated |= val == 1;
            //safety_zoneA (exit side)
            val = (int)((IDictionary)cPLCStatus.rawData["doors"])["safety_zone_B"];
            cPLCStatus.bSafetyZoneActivated |= val == 1;

            TreatFailuresPart(status,cPLCStatus);

            AnalyseAlarmsList(status, cPLCStatus);

            TreatInputPart(status, cPLCStatus); 

            {
                /// State_machine
                //EGRESS' string Egress
                //'EMERGENCY' string Emergency mode
                //'LOCKED_OPEN' string Doors forced by external command
                //'MAINTENANCE' string Maintenance mode
                //'NOMINAL' string Nominal mode
                //'POWER_DOWN' string Power down
                //'TECHNICAL_FAILURE' string Technical failure
                string state_machine_mode = (string)((IDictionary)cPLCStatus.rawData["state_machine"])["mode"];

                eDoorsStatesMachine eDoorsStatesMachine = (eDoorsStatesMachine)Enum.Parse(typeof(eDoorsStatesMachine), state_machine_mode);
                switch (eDoorsStatesMachine)
                {
                    case eDoorsStatesMachine.NOMINAL:
                        #region Nominal
                        {
                            //_plcstatus.eplcState = ePLCState.INSERVICE;
                            //check for doors nominal modes 
                            //'CLOSE_DOOR' string The passenger has crossed the obstacles or a timeout has occured : closing the doors
                            //'FRAUD' string A fraud has occured
                            //'INTRUSION' string An intrusion has occured
                            //'OPEN_DOOR' string Boarding started: the door is open
                            //'WAIT_FOR_AUTHORIZATION'string The gate is ready to accept a new authorisation to start a boarding(or for a person to enter the gate)
                            string machine_state = (string)((IDictionary)cPLCStatus.rawData["state_machine"])["state"];
                            eDoorNominalModes nomMode = (eDoorNominalModes)Enum.Parse(typeof(eDoorNominalModes), machine_state);
                            //Console.WriteLine("machine_state:" + machine_state);
                            #region " Doors Nominal Modes" 
                            //What append if eDoorState is not set ??. Like during intrusion. TOSEE
                            switch (nomMode)
                            {
                                case eDoorNominalModes.OPEN_DOOR:
                                    _plcstatus.eDoorState = eDoorCurrentState.DOOR_OPENED;
                                    break;
                                case eDoorNominalModes.CLOSE_DOOR:
                                    _plcstatus.eDoorState = eDoorCurrentState.DOOR_CLOSED;
                                    break;
                                case eDoorNominalModes.INTRUSION:
                                    _plcstatus.bIntrusion = true;
                                    break;
                                case eDoorNominalModes.FRAUD:
                                    //check for Infractions now eInfractions
                                    _plcstatus.eDoorState = eDoorCurrentState.FRAUD;
                                    _plcstatus.bFraud = true;
                                    break;
                                case eDoorNominalModes.WAIT_FOR_AUTHORIZATION:
                                    break;

                            }//switch (nomMode)
                            #endregion
                            //infractions
                            _plcstatus.bPassageClearTimeout = false;
                            _plcstatus.bPasageCancelled = false;
                            _plcstatus.infractions = 0x00;

                            object[] objinfracs = (object[])cPLCStatus.rawData["alarms"];
                            foreach (object obj in objinfracs)
                            {
                                PasageError = true;
                                _NoOfAuthorization = 0;
                                string inf = (string)(obj);
                                _plcstatus.infractions |= (short)((eInfractions)Enum.Parse(typeof(eInfractions), inf));

                                // intrusion
                                //if ((eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_INTRUSION_A || (eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_INTRUSION_B
                                //    || (eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_OPPOSITE_INTRUSION_A || (eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_OPPOSITE_INTRUSION_B)
                                //    _plcstatus.bWWIntrusion = true;

                                // new changes for intrusion / fraud
                                //switch (_plcstatus.infractions)
                                //{
                                //    case (short)eInfractions.LANG_INTRUSION_A:
                                //    case (short)eInfractions.LANG_OPPOSITE_INTRUSION_A:
                                //        _plcstatus.bInfractionsDirection = 1;// intrusion in exit side
                                //        break;
                                //    case (short)eInfractions.LANG_INTRUSION_B:
                                //    case (short)eInfractions.LANG_OPPOSITE_INTRUSION_B:
                                //        _plcstatus.bInfractionsDirection = 0;// intrusion in entry side
                                //        break;
                                //    case (short)eInfractions.LANG_FRAUD_A:
                                //        _plcstatus.bInfractionsDirection = 1;  // fraud exit side
                                //        break;
                                //    case (short)eInfractions.LANG_FRAUD_B:
                                //        _plcstatus.bInfractionsDirection= 0; // fraud  entry side
                                //        break;
                                //}

                                // these 2 receiving at time of intusion when gate is idle need to check where to add this
                                //LANG_PREALARM_B
                                //LANG_PREALARM_A
                                //if ((eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_FRAUD_A || (eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_FRAUD_B)
                                //    _plcstatus.bUnAuthrizedPassage = true;
                                //Console.WriteLine("========================================================\nAlarms:" + inf);
                                //Logging.Information(LogContext, "IERPLCMAIN.DecodeStatus.Alarms: " + inf);
                            }

                            if (status.alarms.Count >0)
                            {
                                PasageError = true;
                                _NoOfAuthorization = 0;
                            }

                            //check for timeouts
                            //
                            object[] objtimeouts = (object[])cPLCStatus.rawData["timeouts"];
                            if (objtimeouts.Length > 0)
                            {
                                PasageError = true;
                                _NoOfAuthorization = 0;
                                foreach (object objtimeout in objtimeouts)
                                {
                                    string timeout = (string)(objtimeout);
                                    ePassageTimeouts passageTimeouts = (ePassageTimeouts)Enum.Parse(typeof(ePassageTimeouts), timeout);
                                    switch (passageTimeouts)
                                    {
                                        case ePassageTimeouts.LANG_ENTRY_TIMEOUT_A:
                                        case ePassageTimeouts.LANG_ENTRY_TIMEOUT_B:
                                        case ePassageTimeouts.LANG_EXIT_TIMEOUT:
                                        case ePassageTimeouts.LANG_NO_CROSSING_TIMEOUT:
                                            _plcstatus.bPassageClearTimeout = true;
                                            break;
                                        case ePassageTimeouts.LANG_NO_ENTRY_TIMEOUT:
                                            _plcstatus.bPassageClearTimeout = true;
                                            break;
                                    }// switch(passageTimeouts)
                                    Console.WriteLine("========================================================\nTimeout:" + timeout);
                                    Logging.Verbose(LogContext, "IERPLCMAIN.DecodeStatus.Timeouts: " + timeout);
                                }
                            }
                            else
                            {
                                //TOSEE tests of _NoAuthorisation. Has been set previously to A or B. and now testing it is different.
                                //doubtful of result which will be always same.
                                if (!PasageError && ((AuthorizationA + AuthorizationB) != _NoOfAuthorization) && machine_state.ToUpper() != "OPEN_DOOR")
                                {
                                    _NoOfAuthorization = AuthorizationA > 0 ? AuthorizationA : (AuthorizationB > 0 ? AuthorizationA : 0);
                                    _plcstatus.bPasageSuccess = true;
                                    Console.WriteLine("========================================================\nPasageSuccess:Success");
                                    Logging.Verbose(LogContext, "IERPLCMAIN.DecodeStatus.Passage Success");
                                }
                                else
                                {
                                    Logging.Verbose(LogContext, "IERPLCMAIN.DecodeStatus.else of passage success");
                                }
                            }
                        }
                        break;
                    #endregion

                    case eDoorsStatesMachine.MAINTENANCE:
                        //Mode calculated after.
                        //_plcstatus.eplcState = ePLCState.MAINTENANCE;
                        break;
                    case eDoorsStatesMachine.EMERGENCY:
                        //Mode calculated after.
                        //_plcstatus.eplcState = ePLCState.EMERGENCY;
                        break;
                    case eDoorsStatesMachine.TECHNICAL_FAILURE:
                        #region "Failure"
                        //_plcstatus.eplcState = ePLCState.FAILURE;
                        {
                            //Redundant with 
                            //_NoOfAuthorization = AuthorizationA > 0 ? AuthorizationA : (AuthorizationB > 0 ? AuthorizationA : 0);
                            //Following structure will receive all defaults. Major as key major and minor as key minor.
                            //cPLCStatus.failures = new Dictionary<string, string[]>();
                            //there are some failures on the door .. fetch details
                            // major
                            //object[] obj_major_fails = (object[])((IDictionary)cPLCStatus.rawData["failures"])["major"];
                            //object[] obj_minor_fails = (object[])((IDictionary)cPLCStatus.rawData["failures"])["minor"];
                            ////int cnt = major_fails.

                            //string[] majors = new string[obj_major_fails.Length];
                            //string[] minors = new string[obj_minor_fails.Length];
                            //int i = 0;
                            //foreach (object major_fail in obj_major_fails)
                            //{
                            //    // cPLCStatus.failures[]
                            //    majors[i++] = (string)major_fail;
                            //}
                            ////JL i should be reset to 0. Will make crash if not
                            //i = 0;
                            //foreach (object minor_fail in obj_minor_fails)
                            //{
                            //    // cPLCStatus.failures[]
                            //    minors[i++] = (string)minor_fail;
                            //}
                            //cPLCStatus.failures["major"] = majors;
                            //cPLCStatus.failures["minor"] = minors;
                            //Normally it should be already set I think. 
                            _plcstatus.bDoorFailure = true;
                        }
                        #endregion
                        break;
                    case eDoorsStatesMachine.LOCKED_OPEN: //doors forced
                        //_plcstatus.eplcState = ePLCState.OUTOFSERVICE;
                        //JL : Not clear. Should been seen precisely on Gate.
                        _plcstatus.bDoorForced = true; //Initialised to false in constructor.
                        break;
                    default:
                        //JL
                        //EGRESS mode is not treated. But what is it ?
                        //POWER_DOWN is not treated. But what to do ?
                        break;

                }
                //if (_gateOldStatus != (int)_plcstatus.eplcState)
                //{
                //    _gateOldStatus = (int)_plcstatus.eplcState;
                //}
                string operating_modeA = (string)((IDictionary)cPLCStatus.rawData["operating_mode"])["A"];
                string operating_modeB = (string)((IDictionary)cPLCStatus.rawData["operating_mode"])["B"];
                var door_mode = cPLCStatus.rawData["door_mode"];
                //Console.WriteLine("door_mode:"+ door_mode);
                eSideOperatingModeGate EntrySide = (eSideOperatingModeGate)Enum.Parse(typeof(eSideOperatingModeGate), operating_modeA);
                eSideOperatingModeGate ExitSide = (eSideOperatingModeGate)Enum.Parse(typeof(eSideOperatingModeGate), operating_modeB);
                eIERDoorModes _doorsMode = (eIERDoorModes)Enum.Parse(typeof(eIERDoorModes), door_mode.ToString());
                //Following function is calculting Mode, EntryMode, ExitMode, Aisle, Direction according to values in parameter.
                IERHelper.TransformFromIER(eDoorsStatesMachine, EntrySide, ExitSide, _doorsMode, _plcstatus);

                cPLCStatus.bDoorsBlockedClosed = false;
                if (_doorsMode == eIERDoorModes.OP_MODE_BLOCK_CLOSED)
                {
                    cPLCStatus.bDoorsBlockedClosed = true;
                    //_plcstatus.eplcState = ePLCState.OUTOFSERVICE;
                }
                //cPLCStatus.sideOperatingModeA = EntrySide;
                //cPLCStatus.sideOperatingModeA = ExitSide;
                if (eDoorsStatesMachine != eDoorsStatesMachine.NOMINAL)
                //if (_plcstatus.eplcState != ePLCState.INSERVICE)
                {
                    _NoOfAuthorization = 0;
                }
            }
            //Fetch the alrams/ infractions 
        }

        /// <summary>
        /// AuthorizePassage
        /// </summary>
        /// <param name="nbpassage"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public STATUS AuthorizePassage(int nbpassage, int direction)
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
            try
            {
                //Logging.Verbose();
                object[] result = CIERRpcHelper.SetAuthorization(param);
                Console.WriteLine("AuthorizePassage:" + result[0].ToString());
                //Logging.Verbose(LogContext, "IERPLCMain.AuthorizePassage.Number :" + result[0].ToString());
                if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                Logging.Error(LogContext, "CIERPLCMain.AuthorizePassage.WebException : " + ex1.Message);
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception ex)
            {
                Logging.Error(LogContext, "CIERPLCMain.AuthorizePassage.Exception : " + ex.Message);
            }
            return STATUS.NOK;
        }

        public STATUS SetMaintenanceMode(bool onOff)
        {
            object[] result = null;
            try
            {
                int[] paramGateMode = { 0 };
                paramGateMode[0] = onOff ? 1 : 0;
                result = CIERRpcHelper.SetMaintenanceMode(paramGateMode);
                if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                Logging.Error(LogContext, "IERPLCMain.SetMaintenanceMode.WebException " + ex1.Message);
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "IERPLCMain.SetMaintenanceMode.Exception " + e.Message);
            }
            return STATUS.NOK;
        }
        public STATUS SetEmergencyMode(bool onOff)
        {
            object[] result = null;
            try
            {
                int[] paramGateMode = { 0 };
                paramGateMode[0] = onOff ? 1 : 0;
                result = CIERRpcHelper.SetEmergency(paramGateMode);
                if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                Logging.Error(LogContext, "IERPLCMain.SetEmergencyMode.WebException " + ex1.Message);
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "IERPLCMain.SetEmergencyMode.Exception " + e.Message);
            }
            return STATUS.NOK;
        }
        /// <summary>
        /// SetEquipmentMode
        /// </summary>
        /// <param name="0=off / 1=on "></param>
        /// <param name="ePLCStates"></param>
        /// <param name="modeBuffer"></param>
        /// <returns></returns>
        public STATUS SetEquipmentMode(ChangeEquipmentMode EM)
        {
            try
            {
                //int[] paramGateMode = { OnOff };
                object[] result = null;
                string[] modeBuffer = new string[3];

                    switch (EM.Mode)
                    {
                        case GlobalEquipmentMode.OutOfService:
                        case GlobalEquipmentMode.OutOfOrder:
                            modeBuffer[0] = "BlockClosed";
                            modeBuffer[1] = "";
                            modeBuffer[2] = "";
                            Logging.Verbose(LogContext, "SetEquipmentMode " + modeBuffer[0] + " " + modeBuffer[1] + " " + modeBuffer[2]);
                            SetDoorMode(ref result, modeBuffer);
                            break;
                        case GlobalEquipmentMode.InService:
                            modeBuffer[0] = IERHelper.TransformFromChangeModeDoorsMode(EM).ToString();
                            modeBuffer[1] = IERHelper.TransformFromChangeModeSideA(EM).ToString();
                            modeBuffer[2] = IERHelper.TransformFromChangeModeSideB(EM).ToString();
                            Logging.Verbose(LogContext, "SetEquipmentMode " + modeBuffer[0] + " " + modeBuffer[1] + " " + modeBuffer[2]);
                            SetDoorMode(ref result, modeBuffer);
                            break;
                    }

                if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                Logging.Error(LogContext, "IERPLCMain.SetEquipmentMode.WebException " + ex1.Message);
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "IERPLCMain.SetEquipmentMode.Exception " + e.Message);
            }
            return STATUS.NOK;
        }

        /// <summary>
        /// SetCredentials
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public STATUS SetCredentials(string userName, string Password)//On=1, off=0
        {
            try
            {
                string[] param = new string[2];
                param[0] = userName;


                string salt = GenerateRandomAndHashed(Password);
                param[1] = salt;

                object[] result = CIERRpcHelper.SetCredentials(param);
                if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                Console.WriteLine(ex1.Message);
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return STATUS.NOK;
        }

        /// <summary>
        /// Generate Random And Hashed
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private string GenerateRandomAndHashed(string pwd)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, 8)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            //===================================================================================================
            //string saltstr = "$6$" + randomString + "$";

            // byte[] salt = System.Text.Encoding.UTF8.GetBytes(randomString);
            //string str = string.Empty;


            //System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();

            //Byte[] EncryptedSHA512 = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(string.Concat(pwd, salt)));

            //sha512.Clear();

            //return salt+Convert.ToBase64String(EncryptedSHA512);

            // HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            //const int keySize = 64;
            // const int iterations = 350000;

            // salt = RandomNumberGenerator.GetBytes(keySize);
            // var hash = Rfc2898DeriveBytes.Pbkdf2(
            //     Encoding.UTF8.GetBytes(pwd),
            //     salt,
            //     iterations,
            //     hashAlgorithm,
            //     keySize);
            //return "$6$" + randomString + "$"+Convert.ToHexString(hash);

            //return str;



            //System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();

            //Byte[] EncryptedSHA512 = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(string.Concat(pwd, randomString)));

            //sha512.Clear();
            //string hashing = string.Join("", EncryptedSHA512);
            //string hashing = string.Empty;
            //byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);
            //byte[] saltBytes = Encoding.UTF8.GetBytes(randomString);

            //byte[] saltedPasswordBytes = new byte[passwordBytes.Length + saltBytes.Length];
            //Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, 0, passwordBytes.Length);
            //Buffer.BlockCopy(saltBytes, 0, saltedPasswordBytes, passwordBytes.Length, saltBytes.Length);

            //using (var sha512 = SHA512.Create())
            //{
            //    byte[] hashedBytes = sha512.ComputeHash(saltedPasswordBytes);
            //    hashing = Convert.ToBase64String(hashedBytes);
            //}


            //======================================================================
            //byte[] saltBytes = new byte[16]; // Generate a 16-byte salt
            //using (var rngCsp = new RNGCryptoServiceProvider())
            //{
            //    rngCsp.GetBytes(saltBytes);
            //}
            //string saltkey= Convert.ToBase64String(saltBytes);


            string hashedPassword = GenerateHashedPassword(pwd, randomString);

            return "$6$" + randomString + "$" + hashedPassword;
        }

        /// <summary>
        /// GenerateHashedPassword
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GenerateHashedPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];

            Array.Copy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Array.Copy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            using (var sha512 = SHA512.Create())
            {
                byte[] hashedBytes = sha512.ComputeHash(combinedBytes);
                string hashedPassword = Convert.ToBase64String(hashedBytes);

                return hashedPassword;
            }
        }

        /// <summary>
        /// SetDoorMode
        /// </summary>
        /// <param name="result"></param>
        /// <param name="paramGateMode"></param>
        /// <returns></returns>
        public STATUS SetDoorMode(ref object[] result, string[] paramGateMode)//On=1, off=0
        {
            try
            {
                result = CIERRpcHelper.SetMode(paramGateMode);
                if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                Logging.Error(LogContext, "SetDoorMode.WebException " + ex1.Message);
                try { if (result != null && result.Length > 1) Logging.Error(LogContext, "SetDoorMode.Result " + ((int)result[0]).ToString()); } catch { }
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception e)
            {
                Logging.Error(LogContext,"SetDoorMode.Error " + e.Message);
                try { if (result != null && result.Length > 1) Logging.Error(LogContext, "SetDoorMode.Result " + ((int)result[0]).ToString()); } catch { }
            }
            return STATUS.NOK;
        }

        /// <summary>
        /// ConfigurePassage
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public STATUS ConfigurePassage(IERPLCConfigurationData conf, IERPLCConfigurationData2 confInternal)
        {
            bool error = false;
            try
            {
                int[] param = new int[9];
                object[] result;
                if (conf != null)
                {
                    param[0] = conf.FlapRemainOpenPostFreePasses; //Time the obstacles remain open after a user leaves the lane(in Free Mode).
                    param[1] = -1; // not int use
                    param[2] = conf.TimeToEnterAfterAuthorisation;              //Time allotted to enter in the lane after an authorisation is granted
                    param[3] = -1;// not in use
                    param[4] = conf.TimeToValidateAfterDetection;                  //Time allotted to badge after a user is detected in the lane (in Controlled Mode only)
                    param[5] = -1;// not int use
                    param[6] = conf.TimeToCrossAfterDetection; //Time allotted to completely cross the lane after a person is detected.
                    param[7] = conf.TimeAllowedToExitSafetyZone;             //Time allotted to exit the safety zone.
                    param[8] = conf.TimeAllowedToCrossLaneAfterAuthorisation;         //Time allotted to completely cross the lane after an authorisation is granted

                    result = CIERRpcHelper.GetSetTempo(param);
                    if (result.Length < 9) error = true;
                    else
                    {
                        //We could verify the values if they are corresponding to what we have set
                    }
                    Thread.Sleep(50);

                    int[] param2 = new int[2];
                    param2[0] = conf.FlapRemainOpenPostFreePasses; //Time the obstacles remain open after a user leaves the lane(in Free Mode).
                    param2[1] = conf.FlapRemainOpenPostControlledPasses;  //Time the obstacles remain open after a user leaves the lane(Controlled Mode).
                    result = CIERRpcHelper.GetSetTempoFlow(param2);
                    if (result.Length < 2) error = true;
                    else
                    {
                        //We could verify the values if they are corresponding to what we have set
                    }

                    //Motor Configuration
                    object[] param3 = new object[7];
                    param3[0] = "Entry";
                    param3[1] = conf.StandardOpeningSpeed;
                    param3[2] = conf.StandardClosingSpeed;
                    param3[3] = conf.SecurityOpeningSpeed;
                    param3[4] = conf.DisappearanceSpeed;
                    param3[5] = conf.FraudClosingSpeed;
                    param3[6] = conf.SecurityFraudClosingSpeed;
                    result = CIERRpcHelper.SetMotorSpeed(param3);
                    if (result.Length < 1) error = true;
                    else if (int.Parse(result[0].ToString()) <= 0) error = true;
                    {
                        //We could verify the values if they are correct
                    }
                }
                int[] param4 = new int[1];
                int[] param5 = new int[2];
                int[] param6 = new int[2];
                if (confInternal != null)
                {
                    param4[0] = confInternal.ModeBuzzer;
                    param5[0] = confInternal.BuzzerFraudVolume;
                    param5[1] = confInternal.BuzzerFraudNote;
                    param6[0] = confInternal.BuzzerIntrusionVolume;
                    param6[1] = confInternal.BuzzerIntrusionNote;
                    result = CIERRpcHelper.SetBuzzerMode(param4);
                    if (result.Length < 1) error = true;
                    else if (int.Parse(result[0].ToString()) <= 0) error = true;
                    {
                        //We could verify the values if they are correct
                    }
                    result = CIERRpcHelper.SetBuzzerFraud(param5);
                    if (result.Length < 1) error = true;
                    else if (int.Parse(result[0].ToString()) <= 0) error = true;
                    {
                        //We could verify the values if they are correct
                    }
                    result = CIERRpcHelper.SetBuzzerIntrusion(param6);
                    if (result.Length < 1) error = true;
                    else if (int.Parse(result[0].ToString()) <= 0) error = true;
                    {
                        //We could verify the values if they are correct
                    }
                }
                if (!error) return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                Logging.Error(LogContext, "IERPLCMain.ConfigurePassage.WebException " + ex1.Message);
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "IERPLCMain.ConfigurePassage.Exception " + e.Message);
            }
            return STATUS.NOK;
        }


        public STATUS SetFirmware(string fileName)
        {
            try
            {
                //The Gate shall have been previously in OutOfService Mode
                FileTransfertConfiguration conf = new FileTransfertConfiguration();
                conf.Password = "upload";
                conf.Login = "upload";
                conf.Mode = FileTransfertMode.SftpWinscp;
                conf.Root = "update";
                conf.ServerUrl = "192.168.0.200";
                WinSCP_FileTransfert fileTransfert = new WinSCP_FileTransfert(conf);
                fileTransfert.PutFile(fileName);
                //Fingerprint ? ask IER documentation
                //We will wait a little before applying change.
                //Question to see : Have we applied passwork
                object[] result = CIERRpcHelper.ApplyUpdate();
                return STATUS.OK;
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "IERPLCMain.SetFirmware.Exception " + e.Message);
            }
            return STATUS.NOK;
        }

        public Dictionary<int,int> GetCounters(int type)
        {
            Dictionary<int,int> counters = new Dictionary<int,int>();
            try
            {
                int nb = 0;
                object[] result = CIERRpcHelper.GetCounter();
                //First parameter is number of counter
                if (result.Length > 0) nb = Convert.ToInt32(((int)result[0]));
                if (result.Length > 1)
                {
                    IList idict = (IList)result[1];
                    foreach(IDictionary idict1 in idict)
                    {
                        try
                        {
                            int val = 0;
                            switch (type)
                            {
                                case 1:
                                    val = (int)idict1["Partial"];
                                    break;
                                case 2:
                                    val = (int)idict1["Main"];
                                    break;
                                case 3:
                                    val = (int)idict1["Perp"];
                                    break;
                            }
                            string name = (string)idict1["name"];
                            int cpt = Convert.ToInt32(name) ;
                            switch (cpt)
                            {
                                case 1048:
                                    cpt = 14;
                                    break;
                                case 1052:
                                    cpt = 15;
                                    break;
                                case 1076:
                                    cpt = 16;
                                    break;
                                case 1077:
                                    cpt = 17;
                                    break;
                                default:
                                    cpt -= 1000;
                                    break;
                            }
                            counters.Add(cpt, val);
                        }
                        catch (Exception e1)
                        {
                            Logging.Error(LogContext, "IERPLCMain.GetCounters " + e1.Message);
                        }
                    }
                }
                else
                {
                    nb = 0;
                }
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "IERPLCMain.GetCounter.Exception " + e.Message);
            }
            return counters;
        }

        /// <summary>
        /// SetDateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public STATUS SetDateTime(DateTime dt)
        {
            try
            {
                object[] result = CIERRpcHelper.SetDate(dt, _timeZone);
                if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
                return STATUS.NOK;
            }
            catch (System.Net.WebException ex1)
            {
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception e)
            {
                Logging.Error(LogContext, "IERPLCMain.SetDateTime.Exception " + e.Message);
            }
            return STATUS.NOK;
        }


        /// <summary>
        /// GetDateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public STATUS GetDateTime(out DateTime dateTime)
        {
            dateTime = DateTime.Now;
            try
            {
                object[] resp = CIERRpcHelper.GetDate();

                dateTime = new DateTime((int)resp[0], (int)resp[1], (int)resp[2], (int)resp[3], (int)resp[4], (int)resp[5]);
                dateTime.AddSeconds((int)resp[6]);
                return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception ex)
            {
                Logging.Error(LogContext, "IERPLCMain.GetDateTime.Exception " + ex.Message);
            }
            return STATUS.NOK;
        }

        /// <summary>
        /// Set FlapOperationl Mode
        /// </summary>
        /// <param name="FlapsMode"></param>
        /// <param name="OperatingModes_EntrySide"></param>
        /// <param name="OperatingModes_ExitSide"></param>
        /// <returns></returns>
        public STATUS SetFlapOperationlMode(string FlapsMode, string OperatingModes_EntrySide, string OperatingModes_ExitSide)
        {
            string[] paramGateMode = new string[3];
            paramGateMode[0] = FlapsMode;
            paramGateMode[1] = OperatingModes_EntrySide;
            paramGateMode[2] = OperatingModes_ExitSide;
            try
            {
                object[] result = CIERRpcHelper.SetMode(paramGateMode);
                if (int.Parse(result[0].ToString()) > 0) return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception ex)
            { }
            return STATUS.NOK;
        }

        /// <summary>
        /// MethodHelp
        /// </summary>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public string MethodHelp(string MethodName)
        {
            string result = string.Empty;
            try
            {
                return result = CIERRpcHelper.methodHelp(MethodName);
            }
            catch (System.Net.WebException ex1)
            {
                return ex1.Message;
            }
            catch (Exception ex)
            { }
            return STATUS.NOK.ToString();
        }

        /// <summary>
        /// light management for IER gate
        /// </summary>
        /// <returns></returns>
        public STATUS ManagePassageLight(int EntryReader, int ExitReader)
        {
            int[] param = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            try
            {
                param[LocalConfiguration.ReadIntParameter("EntryReaderLightNumber", 3)] = EntryReader;
                param[LocalConfiguration.ReadIntParameter("ExitReaderLightNumber",2)] = ExitReader;

                object[] result = CIERRpcHelper.SetOutputClient(param);
                Console.WriteLine("ManagePassageLight:" + result.Length);
                string str = "";
                for (int i = 0; i < result.Length; i++)
                {
                    str+= ((int)result[i]).ToString()+";";
                }
                Logging.Verbose(LogContext, "CIERPLCMain.ManagePassageLight result : " + result.Length+" "+str);
                if (int.Parse(result[0].ToString()) > 0)
                    return STATUS.OK;
            }
            catch (System.Net.WebException ex1)
            {
                Logging.Error(LogContext, "CIERPLCMain.ManagePassageLight.WebException : " + ex1.Message);
                return STATUS.GCU_COMM_ERROR;
            }
            catch (Exception ex)
            {
                Logging.Error(LogContext, "CIERPLCMain.ManagePassageLight.Exception : " + ex.Message);
            }
            return STATUS.NOK;
        }
    }

    internal class WinSCP_FileTransfert
    {
        private FileTransfertConfiguration conf;

        public WinSCP_FileTransfert(FileTransfertConfiguration conf)
        {
            this.conf = conf;
        }

        internal void PutFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
