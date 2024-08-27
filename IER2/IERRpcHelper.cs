using Domain.Peripherals.Passage;
using EtGate.IER;
using IFS2.Common;
using IFS2.Common.CoreTechnical;
using IFS2.Equipment.DriverInterface;
using LanguageExt;
using System.Collections;
using System.Xml.Serialization;

using GlobalEquipmentDirection = IFS2.Equipment.DriverInterface.GlobalEquipmentDirection;
using GlobalEquipmentEntryMode = IFS2.Equipment.DriverInterface.GlobalEquipmentEntryMode;
using GlobalEquipmentExitMode = IFS2.Equipment.DriverInterface.GlobalEquipmentExitMode;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager;

public partial class CIERRpcHelper
{
    public CIERRpcHelper(IERXmlRpcRaw _iERXmlRpcInterface)
    {
        this.xmlRpcRaw = _iERXmlRpcInterface;
    }

    private IIERXmlRpcRaw xmlRpcRaw;

    public static Dictionary<string, object> ResponseParser(object obj)
    {
        Dictionary<string, object> dict = new();
        
        if (typeof(IDictionary).IsAssignableFrom(obj.GetType()))
         {
            IDictionary idict = (IDictionary)obj;

            foreach (object key in idict.Keys)
            {
                if (typeof(IDictionary).IsAssignableFrom(idict[key].GetType()))
                {
                    var di = ResponseParser(idict[key]);
                    if (di.Count > 0)
                        dict.Add(objToString(key), di);                    
                }                
                else                
                    dict.Add(objToString(key), idict[key]);                
            }
        }
        
        return dict;
    }

    private static string objToString(object obj)
    {
        string str = "";
        string type = obj.GetType().FullName;
        if (obj.GetType().FullName == "System.String")
        {
            str = (string)obj;
        }
        /*  else if (obj.GetType().FullName == "test.Testclass")
           {
               TestClass c = (TestClass)obj;
               str = c.Info;
           }*/
        return str;
    }        

    public bool SetAuthorisation(int nbpassage, int direction)
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
        
        return xmlRpcRaw.SetAuthorisation(param)
            .Match(Some: o => successGenCriter(o),
                None: () => false
                );
    }
    
    public bool SetEmergency(bool onOff)
    {
        int[] paramGateMode = { 0 };
        paramGateMode[0] = onOff ? 1 : 0;
        return xmlRpcRaw.SetEmergency(paramGateMode)
            .Match(Some: o => successGenCriter(o),
                None: () => false
                );
    }
    
    public bool SetMaintenanceMode(bool onOff)
    {
        int[] paramGateMode = { 0 };
        paramGateMode[0] = onOff ? 1 : 0;

        return xmlRpcRaw.SetMaintenanceMode(paramGateMode)
            .Match(Some: o => successGenCriter(o),
                None: () => false
                );
    }

    readonly Func<object[], bool> successGenCriter = (object[] op) =>
    op.Length > 1
    && int.TryParse(op[0].ToString(), out int res)
    && res > 0;


    public bool Reboot(bool bhardboot)
    {        
        if (bhardboot)
            return xmlRpcRaw.Reboot().Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
        else
            return xmlRpcRaw.Restart().Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }

    public bool SetFirmware(string fileName)
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
        }
        catch
        {
            return false;
        }
        //Fingerprint ? ask IER documentation
        //We will wait a little before applying change.
        //Question to see : Have we applied passwork
        return xmlRpcRaw.ApplyUpdate().Match(
            Some: _ => true, 
            None: () => false
            );
        }    

    public bool SetDate(DateTime dt, string timezone = "")
    {
        object[] dtparams = new object[7];
        dtparams[0] = (int)dt.Year;
        dtparams[1] = (int)dt.Month;
        dtparams[2] = (int)dt.Day;
        dtparams[3] = (int)dt.Hour;
        dtparams[4] = (int)dt.Minute;
        dtparams[5] = (int)dt.Second;
        dtparams[6] = (string)timezone;// dt.timezo;
        //for test
        //Logging.Verbose(new LogContext("Log","",""), ((int)dtparams[0]).ToString(), ((int)dtparams[1]).ToString(), ((int)dtparams[2]).ToString(), ((int)dtparams[3]).ToString(), ((int)dtparams[4]).ToString(), ((int)dtparams[5]).ToString(), ((string)dtparams[6]).ToString());
        return xmlRpcRaw.SetDate(dtparams).Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }

    public Option<DateTime> GetDate()
    {
        var dateExtract = (object[] resp) =>
            resp.Length > 7 ?
            (new DateTime
            ((int)resp[0], (int)resp[1], (int)resp[2], (int)resp[3], (int)resp[4], (int)resp[5])).AddSeconds((int)resp[6])
             : Option<DateTime>.None;

        return xmlRpcRaw.GetVersion().Bind(dateExtract);        
    }

    public class ForFailure
    {
        [XmlElement("key")]
        public string key;
        [XmlElement("failure")]
        public List<string> list;
        public ForFailure() { }

        public ForFailure(string key_, List<string> value_)
        {
            key = key_;
            list = value_;
        }
    }

    public class DictionaryXmlString
    {
        [XmlElement("key")]
        public string key;
        [XmlElement("value")]
        public string value;

        public DictionaryXmlString() { }

        public DictionaryXmlString(string key_, string value_)
        {
            key = key_;
            value = value_;
        }
    }

    private static void TreatFailuresPart(GetStatusStdAnswer status, CPLCStatus cPLCStatus)
    {
        cPLCStatus.Camera = EventAlarmLevel.Normal;
        cPLCStatus.CameraDetail = "";
        cPLCStatus.CanBus = EventAlarmLevel.Normal;
        cPLCStatus.CanBusDetail = "";
        cPLCStatus.Sensors = EventAlarmLevel.Normal;
        cPLCStatus.SensorsDetail = "";
        cPLCStatus.Motor = EventAlarmLevel.Normal;
        cPLCStatus.MotorDetail = "";
        cPLCStatus.Brake = EventAlarmLevel.Normal;
        cPLCStatus.BrakeDetail = "";
        cPLCStatus.GCUElectronic = EventAlarmLevel.Normal;
        cPLCStatus.GCUElectronicDetail = "";
        cPLCStatus.GCUSystem = EventAlarmLevel.Normal;
        cPLCStatus.GCUSystemDetail = "";
        cPLCStatus.AFCCommunication = EventAlarmLevel.Normal;
        cPLCStatus.AFCCommunicationDetail = "";
        cPLCStatus.GCUTemperature = EventAlarmLevel.Normal;
        cPLCStatus.GCUTemperatureDetail = "";
        if (status.failures.Count > 0)
        {
            ForFailure failures = status.failures.Find(x => x.key == "minor");
            if (failures != null)
            {
                AnalyseFailureList(failures.list, EventAlarmLevel.Warning, cPLCStatus);
            }
            failures = status.failures.Find(x => x.key == "major");
            if (failures != null)
            {
                AnalyseFailureList(failures.list, EventAlarmLevel.Alarm, cPLCStatus);
            }
        }
    }

    private static void AnalyseFailureList(List<string> list, EventAlarmLevel level, CPLCStatus status)
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
                    status.Camera = level;
                    status.CameraDetail += (status.CameraDetail != "" ? ";" : "") + failure;
                    break;
                case "ERRORS_CAN":
                case "ERRORS_CAN_HEARTBEAT":
                case "ERRORS_CAN_OVERFLOW":
                case "ERRORS_CAN_PRODUCT_CODE":
                case "ERRORS_CAN_SW_VERSION":
                    status.CanBus = level;
                    status.CanBusDetail += (status.CanBusDetail != "" ? ";" : "") + failure;
                    break;
                case "ERRORS_CELL_IR":
                case "ERRORS_DETECTION":
                case "ERRORS_DIRAS_EMITTER":
                case "ERRORS_DIRAS_RECEIVER":
                case "ERRORS_OBSTRUCTED_CELLS":
                case "ERRORS_LATERAL_DETECTION":
                    status.Sensors = level;
                    status.SensorsDetail += (status.SensorsDetail != "" ? ";" : "") + failure;
                    break;
                case "ERRORS_BLOCKED_MOTOR":
                case "ERRORS_MOTOR_CARD":
                case "ERRORS_MOTOR_CARD_CONFIG":
                case "ERRORS_MOTOR_INIT":
                case "ERRORS_MOTOR_PEAK_CURRENT":
                case "ERRORS_HALL_EFFECT_FAILURE":
                    status.Motor = level;
                    status.MotorDetail += (status.MotorDetail != "" ? ";" : "") + failure;
                    break;
                case "ERRORS_MOTOR_BRAKE":
                case "ERRORS_MOTOR_BRAKE_FAILURE":
                    status.Brake = level;
                    status.BrakeDetail += (status.BrakeDetail != "" ? ";" : "") + failure;
                    break;
                case "ERRORS_INSTALLATION":
                case "ERRORS_CPU":
                case "ERRORS_EXT":
                    status.GCUElectronic = level;
                    status.GCUElectronicDetail += (status.GCUElectronicDetail != "" ? ";" : "") + failure;
                    break;
                case "ERRORS_SYSTEMD_FAILURE":
                case "ERRORS_USINE_TEST_WARNING":
                    status.GCUSystem = level;
                    status.GCUSystemDetail += (status.GCUSystemDetail != "" ? ";" : "") + failure;
                    break;
                case "ERRORS_CUSTOMER":
                case "ERRORS_CUSTOMER_FAILURE":
                    status.AFCCommunication = level;
                    status.AFCCommunicationDetail += (status.AFCCommunicationDetail != "" ? ";" : "") + failure;
                    break;
                case "ERRORS_TEMPERATURE":
                case "ERRORS_THERMOPILE":
                    status.GCUTemperature = level;
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
    private static void AnalyseAlarmsList(GetStatusStdAnswer status, CPLCStatus cPLCStatus)
    {
        cPLCStatus.bWWIntrusionExit = false;
        cPLCStatus.bWWIntrusionEntry = false;
        cPLCStatus.bFraudEntry = false;
        cPLCStatus.bFraudExit = false;
        cPLCStatus.bFraud = false;
        cPLCStatus.bFraudRamp = false;
        cPLCStatus.bFraudOthers = false;
        cPLCStatus.bWWIntrusion = false;
        cPLCStatus.bPreIntrusionEntry = false;
        cPLCStatus.bPreIntrusionExit = false;
        cPLCStatus.bPreIntrusion = false;

        if (status.alarms.Count > 0)
        {
            foreach (string str in status.alarms)
            {
                switch (str)
                {
                    case "LANG_FRAUD_A":
                        //A person has cross the entry without authorisation. => need to display to go back.
                        //On next side prohibited if authorised. If one validation result let it.
                        cPLCStatus.bFraudEntry = true;
                        break;
                    case "LANG_FRAUD_B":
                        //A person has cross the exit without authorisation
                        cPLCStatus.bFraudExit = true;
                        break;
                    case "LANG_FRAUD_HOLDING":
                    case "LANG_FRAUD_MANTRAP":
                    case "LANG_FRAUD_UNEXPECTED_MOTION":
                        cPLCStatus.bFraudOthers = true;
                        break;
                    case "LANG_FRAUD_JUMP":
                        cPLCStatus.bFraudJump = true;
                        break;
                    case "LANG_FRAUD_RAMPING":
                        cPLCStatus.bFraudRamp = true;
                        break;
                    case "LANG_INTRUSION_A":
                    case "LANG_OPPOSITE_INTRUSION_B":
                        //A person stays in the entry. => ask to exit. next side shall be prohibited
                        cPLCStatus.bWWIntrusionEntry = true;
                        break;
                    case "LANG_INTRUSION_B":
                    case "LANG_OPPOSITE_INTRUSION_A":
                        cPLCStatus.bWWIntrusionExit = true;
                        break;
                    case "LANG_PREALARM_A":
                        cPLCStatus.bPreIntrusionEntry = true;
                        break;
                    case "LANG_PREALARM_B":
                        cPLCStatus.bPreIntrusionExit = true;
                        break;
                    case "LANG_FRAUD_DISAPPEARANCE":
                        break;
                }
            }
        }
        cPLCStatus.bFraud = cPLCStatus.bFraudJump || cPLCStatus.bFraudRamp || cPLCStatus.bFraudExit || cPLCStatus.bFraudEntry || cPLCStatus.bFraudOthers;
        cPLCStatus.bWWIntrusion = cPLCStatus.bWWIntrusionEntry || cPLCStatus.bWWIntrusionExit;
        cPLCStatus.bPreIntrusion = cPLCStatus.bPreIntrusionEntry || cPLCStatus.bPreIntrusionExit;
    }

    public class DictionaryXmlInt
    {
        [XmlElement("key")]
        public string key;
        [XmlElement("value")]
        public int value;

        public DictionaryXmlInt() { }

        public DictionaryXmlInt(string key_, int value_)
        {
            key = key_;
            value = value_;
        }
    }
    private static void TreatInputPart(GetStatusStdAnswer status, CPLCStatus cPLCStatus)
    {
        cPLCStatus.bPoweredOnUPS = false;
        cPLCStatus.bEntryMaintenanceSwitch = false;
        cPLCStatus.bExitMaintenanceSwitch = false;
        cPLCStatus.bEmergencyButton = false;
        DictionaryXmlInt dico = status.inputs.Find(x => x.key == "emergency");
        if ((dico != null) && (dico.value == 1)) cPLCStatus.bEmergencyButton = true;
        dico = status.inputs.Find(x => x.key == "entry_locked_open");
        if ((dico != null) && (dico.value == 1)) cPLCStatus.bEntryMaintenanceSwitch = true;
        dico = status.inputs.Find(x => x.key == "ups");
        if ((dico != null) && (dico.value == 1)) cPLCStatus.bPoweredOnUPS = true;
    }
    public enum eDoorNominalModes //Normal case
    {
        NONE = -1,
        OPEN_DOOR, //Doors are opened
        CLOSE_DOOR, //Doors are closed
        FRAUD, //A Fraud has occured
        INTRUSION, // Intrusion
        WAIT_FOR_AUTHORIZATION //The gate is ready to accept a new authorisation to start a boarding (or for a person to enter the gate)
    }
    private static void DecodeStatusData(CPLCStatus cPLCStatus)
    {
        cPLCStatus.Clear();
        GetStatusStdAnswer status = new(cPLCStatus.rawData);

        bool PasageError = false;
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
        int _NoOfAuthorization = AuthorizationA > 0 ? AuthorizationA : (AuthorizationB > 0 ? AuthorizationB : 0);
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

        TreatFailuresPart(status, cPLCStatus);

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

            Domain.Peripherals.Passage.eDoorsStatesMachine eDoorsStatesMachine = (Domain.Peripherals.Passage.eDoorsStatesMachine)Enum.Parse(typeof(Domain.Peripherals.Passage.eDoorsStatesMachine), state_machine_mode);
            switch (eDoorsStatesMachine)
            {
                case Domain.Peripherals.Passage.eDoorsStatesMachine.NOMINAL:
                    #region Nominal
                    {
                        //cPLCStatus.eplcState = ePLCState.INSERVICE;
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
                                cPLCStatus.eDoorState = eDoorCurrentState.DOOR_OPENED;
                                break;
                            case eDoorNominalModes.CLOSE_DOOR:
                                cPLCStatus.eDoorState = eDoorCurrentState.DOOR_CLOSED;
                                break;
                            case eDoorNominalModes.INTRUSION:
                                cPLCStatus.bIntrusion = true;
                                break;
                            case eDoorNominalModes.FRAUD:
                                //check for Infractions now eInfractions
                                cPLCStatus.eDoorState = eDoorCurrentState.FRAUD;
                                cPLCStatus.bFraud = true;
                                break;
                            case eDoorNominalModes.WAIT_FOR_AUTHORIZATION:
                                break;

                        }//switch (nomMode)
                        #endregion
                        //infractions
                        cPLCStatus.bPassageClearTimeout = false;
                        cPLCStatus.bPasageCancelled = false;
                        cPLCStatus.infractions = 0x00;

                        object[] objinfracs = (object[])cPLCStatus.rawData["alarms"];
                        foreach (object obj in objinfracs)
                        {
                            PasageError = true;
                            _NoOfAuthorization = 0;
                            string inf = (string)(obj);
                            cPLCStatus.infractions |= (short)((eInfractions)Enum.Parse(typeof(eInfractions), inf));

                            // intrusion
                            //if ((eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_INTRUSION_A || (eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_INTRUSION_B
                            //    || (eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_OPPOSITE_INTRUSION_A || (eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_OPPOSITE_INTRUSION_B)
                            //    cPLCStatus.bWWIntrusion = true;

                            // new changes for intrusion / fraud
                            //switch (cPLCStatus.infractions)
                            //{
                            //    case (short)eInfractions.LANG_INTRUSION_A:
                            //    case (short)eInfractions.LANG_OPPOSITE_INTRUSION_A:
                            //        cPLCStatus.bInfractionsDirection = 1;// intrusion in exit side
                            //        break;
                            //    case (short)eInfractions.LANG_INTRUSION_B:
                            //    case (short)eInfractions.LANG_OPPOSITE_INTRUSION_B:
                            //        cPLCStatus.bInfractionsDirection = 0;// intrusion in entry side
                            //        break;
                            //    case (short)eInfractions.LANG_FRAUD_A:
                            //        cPLCStatus.bInfractionsDirection = 1;  // fraud exit side
                            //        break;
                            //    case (short)eInfractions.LANG_FRAUD_B:
                            //        cPLCStatus.bInfractionsDirection= 0; // fraud  entry side
                            //        break;
                            //}

                            // these 2 receiving at time of intusion when gate is idle need to check where to add this
                            //LANG_PREALARM_B
                            //LANG_PREALARM_A
                            //if ((eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_FRAUD_A || (eInfractions)Enum.Parse(typeof(eInfractions), inf) == eInfractions.LANG_FRAUD_B)
                            //    cPLCStatus.bUnAuthrizedPassage = true;
                            //Console.WriteLine("========================================================\nAlarms:" + inf);
                            //Logging.Information(LogContext, "IERPLCMAIN.DecodeStatus.Alarms: " + inf);
                        }

                        if (status.alarms.Count > 0)
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
                                        cPLCStatus.bPassageClearTimeout = true;
                                        break;
                                    case ePassageTimeouts.LANG_NO_ENTRY_TIMEOUT:
                                        cPLCStatus.bPassageClearTimeout = true;
                                        break;
                                }// switch(passageTimeouts)
                                Console.WriteLine("========================================================\nTimeout:" + timeout);                                
                            }
                        }
                        else
                        {
                            //TOSEE tests of _NoAuthorisation. Has been set previously to A or B. and now testing it is different.
                            //doubtful of result which will be always same.
                            if (!PasageError && ((AuthorizationA + AuthorizationB) != _NoOfAuthorization) && machine_state.ToUpper() != "OPEN_DOOR")
                            {
                                _NoOfAuthorization = AuthorizationA > 0 ? AuthorizationA : (AuthorizationB > 0 ? AuthorizationA : 0);
                                cPLCStatus.bPasageSuccess = true;
                                Console.WriteLine("========================================================\nPasageSuccess:Success");
                                
                            }
                            else
                            {
                                
                            }
                        }
                    }
                    break;
                #endregion

                case Domain.Peripherals.Passage.eDoorsStatesMachine.MAINTENANCE:
                    //Mode calculated after.
                    //cPLCStatus.eplcState = ePLCState.MAINTENANCE;
                    break;
                case Domain.Peripherals.Passage.eDoorsStatesMachine.EMERGENCY:
                    //Mode calculated after.
                    //cPLCStatus.eplcState = ePLCState.EMERGENCY;
                    break;
                case Domain.Peripherals.Passage.eDoorsStatesMachine.TECHNICAL_FAILURE:                    
                    break;
                case Domain.Peripherals.Passage.eDoorsStatesMachine.LOCKED_OPEN: //doors forced
                                                      //cPLCStatus.eplcState = ePLCState.OUTOFSERVICE;
                                                      //JL : Not clear. Should been seen precisely on Gate.
                    cPLCStatus.bDoorForced = true; //Initialised to false in constructor.
                    break;
                default:
                    //JL
                    //EGRESS mode is not treated. But what is it ?
                    //POWER_DOWN is not treated. But what to do ?
                    break;

            }
            //if (_gateOldStatus != (int)cPLCStatus.eplcState)
            //{
            //    _gateOldStatus = (int)cPLCStatus.eplcState;
            //}
            string operating_modeA = (string)((IDictionary)cPLCStatus.rawData["operating_mode"])["A"];
            string operating_modeB = (string)((IDictionary)cPLCStatus.rawData["operating_mode"])["B"];
            var door_mode = cPLCStatus.rawData["door_mode"];
            //Console.WriteLine("door_mode:"+ door_mode);
            eSideOperatingModeGate EntrySide = (eSideOperatingModeGate)Enum.Parse(typeof(eSideOperatingModeGate), operating_modeA);
            eSideOperatingModeGate ExitSide = (eSideOperatingModeGate)Enum.Parse(typeof(eSideOperatingModeGate), operating_modeB);
            eIERDoorModes _doorsMode = (eIERDoorModes)Enum.Parse(typeof(eIERDoorModes), door_mode.ToString());
            //Following function is calculting Mode, EntryMode, ExitMode, Aisle, Direction according to values in parameter.
            TransformFromIER(eDoorsStatesMachine, EntrySide, ExitSide, _doorsMode, cPLCStatus);

            cPLCStatus.bDoorsBlockedClosed = false;
            if (_doorsMode == eIERDoorModes.OP_MODE_BLOCK_CLOSED)
            {
                cPLCStatus.bDoorsBlockedClosed = true;
                //cPLCStatus.eplcState = ePLCState.OUTOFSERVICE;
            }
            //cPLCStatus.sideOperatingModeA = EntrySide;
            //cPLCStatus.sideOperatingModeA = ExitSide;
            if (eDoorsStatesMachine != Domain.Peripherals.Passage.eDoorsStatesMachine.NOMINAL)
            //if (cPLCStatus.eplcState != ePLCState.INSERVICE)
            {
                _NoOfAuthorization = 0;
            }
        }
        //Fetch the alrams/ infractions 
        
    }

    public static void TransformFromIER(eDoorsStatesMachine mode, eSideOperatingModeGate entryMode, eSideOperatingModeGate exitMode, eIERDoorModes doorMode, CPLCStatus EM)
    {
        switch (mode)
        {
            case eDoorsStatesMachine.NOMINAL:
                if (doorMode == eIERDoorModes.OP_MODE_BLOCK_CLOSED)
                {
                    EM.Mode = GlobalEquipmentMode.OutOfService;
                    EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                    EM.EntryMode = GlobalEquipmentEntryMode.Closed;
                    EM.ExitMode = GlobalEquipmentExitMode.Closed;

                }
                else
                {
                    EM.Mode = GlobalEquipmentMode.InService;
                    switch (entryMode)
                    {
                        case eSideOperatingModeGate.OP_MODE_SIDE_FREE:
                            EM.EntryMode = GlobalEquipmentEntryMode.Free;
                            break;
                        case eSideOperatingModeGate.OP_MODE_SIDE_CONTROLLED:
                            EM.EntryMode = GlobalEquipmentEntryMode.Controlled;
                            break;
                        case eSideOperatingModeGate.OP_MODE_SIDE_CLOSED:
                            EM.EntryMode = GlobalEquipmentEntryMode.Closed;
                            break;
                    }
                    switch (exitMode)
                    {
                        case eSideOperatingModeGate.OP_MODE_SIDE_FREE:
                            EM.ExitMode = GlobalEquipmentExitMode.Free;
                            break;
                        case eSideOperatingModeGate.OP_MODE_SIDE_CONTROLLED:
                            EM.ExitMode = GlobalEquipmentExitMode.Controlled;
                            break;
                        case eSideOperatingModeGate.OP_MODE_SIDE_CLOSED:
                            EM.ExitMode = GlobalEquipmentExitMode.Closed;
                            break;
                    }
                    switch (doorMode)
                    {
                        case eIERDoorModes.OP_MODE_BLOCK_CLOSED:
                            EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                            break;
                        case eIERDoorModes.OP_MODE_OPTICAL_B:
                            EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                            break;
                        case eIERDoorModes.OP_MODE_NOB:
                            EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                            break;
                        case eIERDoorModes.OP_MODE_NC:
                            EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                            break;
                        case eIERDoorModes.OP_MODE_OPTICAL_A:
                            EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                            break;
                        case eIERDoorModes.OP_MODE_NOA:
                            EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                            break;
                    }
                }
                if (EM.EntryMode == GlobalEquipmentEntryMode.Closed && EM.ExitMode == GlobalEquipmentExitMode.Closed)
                {
                    EM.Mode = GlobalEquipmentMode.OutOfService;
                    EM.Direction = GlobalEquipmentDirection.None;
                    EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                }
                else if (EM.EntryMode == GlobalEquipmentEntryMode.Free || EM.EntryMode == GlobalEquipmentEntryMode.Controlled)
                {
                    if (EM.ExitMode == GlobalEquipmentExitMode.Free || EM.ExitMode == GlobalEquipmentExitMode.Controlled)
                    {
                        EM.Direction = GlobalEquipmentDirection.BiDirectional;
                    }
                    else
                    {
                        EM.Direction = GlobalEquipmentDirection.Entry;
                    }
                }
                else
                {
                    EM.Direction = GlobalEquipmentDirection.Exit;
                }
                break;
            case eDoorsStatesMachine.LOCKED_OPEN:
                EM.Mode = GlobalEquipmentMode.OutOfService;
                EM.Direction = DriverInterface.GlobalEquipmentDirection.None;
                EM.EntryMode = GlobalEquipmentEntryMode.Closed;
                EM.ExitMode = GlobalEquipmentExitMode.Closed;
                EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                break;
            case eDoorsStatesMachine.EMERGENCY:
                EM.Mode = GlobalEquipmentMode.Emergency;
                EM.Direction = GlobalEquipmentDirection.BiDirectional;
                EM.EntryMode = GlobalEquipmentEntryMode.Free;
                EM.ExitMode = GlobalEquipmentExitMode.Free;
                EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                break;
            case eDoorsStatesMachine.MAINTENANCE:
                EM.Mode = GlobalEquipmentMode.Maintenance;
                break;
            case eDoorsStatesMachine.TECHNICAL_FAILURE:
            //To verify what to do in case of POWER_DOWN. Certainly not OK.
            case eDoorsStatesMachine.POWER_DOWN:
                EM.Mode = GlobalEquipmentMode.OutOfOrder;
                EM.Direction = GlobalEquipmentDirection.None;
                EM.EntryMode = GlobalEquipmentEntryMode.Closed;
                EM.ExitMode = GlobalEquipmentExitMode.Closed;
                EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                break;
                //EGRESS mode not treated. To see what is usage of this.
        }
    }

    public Option<CPLCStatus> GetStatusEx()
    {
        var extractor = (object result) => {
            try
            {
                CPLCStatus cPLCStatus = new();
                cPLCStatus.Reset();

                cPLCStatus.rawData = CIERRpcHelper.ResponseParser(result);
                if (cPLCStatus.rawData.Count > 0)
                    DecodeStatusData(cPLCStatus);
                return cPLCStatus;
            }
            catch
            {
                return Option<CPLCStatus>.None;
            }
        };
        return xmlRpcRaw.GetStatusEx().Bind(extractor);
    }

    public Option<IERSWVersion> GetVersion()
    {
        var versionExtract = (object[] op) =>
            op.Length > 5 ?
            new IERSWVersion {
                LaneType = (string)op[0],
                SWVersion = (string)op[1],
                CompilationDate = (string)op[2],
                GITVersion = (string)op[3],
                GITDate = (string)op[4]
            } : Option<IERSWVersion>.None;

        return xmlRpcRaw.GetVersion().Bind(versionExtract);
    }

    public Option<Dictionary<int, int>> GetCounter()
    {
        var countersExtract = (object[] result) =>
        {
            int nb;
            Dictionary<int, int> counters = new();
            if (result.Length > 0) nb = Convert.ToInt32(((int)result[0]));
            if (result.Length > 1)
            {
                IList idict = (IList)result[1];
                foreach (IDictionary idict1 in idict)
                {
                    try
                    {
                        int val = 0;
                        const int type = 2; // Taking from the matured SGP, where only value for `type` used is 2
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
                        int cpt = Convert.ToInt32(name);
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
                    catch
                    {
                        //   Logging.Error(LogContext, "IERPLCMain.GetCounters " + e1.Message);
                    }
                }
            }
            return counters;
        };

        return xmlRpcRaw.GetCounter().Map(countersExtract);
    }
    
    public bool SetFlapOperationlMode(string FlapsMode, string OperatingModes_EntrySide, string OperatingModes_ExitSide)
    {
        string[] paramGateMode = new string[3];
        paramGateMode[0] = FlapsMode;
        paramGateMode[1] = OperatingModes_EntrySide;
        paramGateMode[2] = OperatingModes_ExitSide;

        return xmlRpcRaw.SetMode(paramGateMode)
            .Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }

    public bool SetOOO()
    {
        return SetDoorlMode("BlockClosed", "", "");
    }

    public bool SetInservice(ChangeEquipmentMode EM)
    {
        DoorModeConf doorMode = new(ForceDoorinReverseSide:false, ForceDoorinEntrySide:true, ForceDoorinFreeSide:true);
        return SetDoorlMode(TransformFromChangeModeDoorsMode(EM, doorMode).ToString(), TransformFromChangeModeSideA(EM).ToString(), TransformFromChangeModeSideB(EM).ToString());
    }

    bool SetDoorlMode(string FlapsMode, string OperatingModes_EntrySide, string OperatingModes_ExitSide)
    {
        string[] paramGateMode = new string[3];
        paramGateMode[0] = FlapsMode;
        paramGateMode[1] = OperatingModes_EntrySide;
        paramGateMode[2] = OperatingModes_ExitSide;

        return xmlRpcRaw.SetMode(paramGateMode)
            .Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }

    public Option<object[]> SetCredentials(string[] param)
    {
        return xmlRpcRaw.SetCredentials(param);
    }
    static SideOperatingModes TransformFromChangeModeSideA(ChangeEquipmentMode EM)
    {
        SideOperatingModes _SideOperationModeA = SideOperatingModes.Closed;
        if (EM.Mode == GlobalEquipmentMode.InService)
        {
            switch (EM.EntryMode)
            {
                case GlobalEquipmentEntryMode.Controlled:
                    if (EM.Direction == DriverInterface.GlobalEquipmentDirection.Entry || EM.Direction == DriverInterface.GlobalEquipmentDirection.BiDirectional)
                        _SideOperationModeA = SideOperatingModes.Controlled;
                    break;
                case GlobalEquipmentEntryMode.Free:
                case GlobalEquipmentEntryMode.FreeControlled:
                    if (EM.Direction == DriverInterface.GlobalEquipmentDirection.Entry || EM.Direction == DriverInterface.GlobalEquipmentDirection.BiDirectional)
                        _SideOperationModeA = SideOperatingModes.Free;
                    break;
            }
        }
        return _SideOperationModeA;
    }
    static SideOperatingModes TransformFromChangeModeSideB(ChangeEquipmentMode EM)
    {
        SideOperatingModes _SideOperationModeB = SideOperatingModes.Closed;
        if (EM.Mode == GlobalEquipmentMode.InService)
        {
            switch (EM.ExitMode)
            {
                case GlobalEquipmentExitMode.Controlled:
                    if (EM.Direction == DriverInterface.GlobalEquipmentDirection.Exit || EM.Direction == DriverInterface.GlobalEquipmentDirection.BiDirectional)
                        _SideOperationModeB = SideOperatingModes.Controlled;
                    break;
                case GlobalEquipmentExitMode.Free:
                case GlobalEquipmentExitMode.FreeControlled:
                    if (EM.Direction == DriverInterface.GlobalEquipmentDirection.Exit || EM.Direction == DriverInterface.GlobalEquipmentDirection.BiDirectional)
                        _SideOperationModeB = SideOperatingModes.Free;
                    break;
            }
        }
        return _SideOperationModeB;
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

    public bool GetSetTempo(TempoConf conf)
    {
        int[] param = new int[9];

        param[0] = conf.FlapRemainOpenPostFreePasses;
        param[1] = -1; // not int use
        param[2] = conf.TimeToEnterAfterAuthorisation;
        param[3] = -1;// not in use
        param[4] = conf.TimeToValidateAfterDetection;
        param[5] = -1;// not int use
        param[6] = conf.TimeToCrossAfterDetection;
        param[7] = conf.TimeAllowedToExitSafetyZone;
        param[8] = conf.TimeAllowedToCrossLaneAfterAuthorisation;

        return xmlRpcRaw.GetSetTempo(param).Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }

    public bool GetSetTempoFlow(TempoFlowConf conf)
    {
        int[] param2 = new int[2];
        param2[0] = conf.FlapRemainOpenPostFreePasses; //Time the obstacles remain open after a user leaves the lane(in Free Mode).
        param2[1] = conf.FlapRemainOpenPostControlledPasses;  //Time the obstacles remain open after a user leaves the lane(Controlled Mode).

        return xmlRpcRaw.GetSetTempoFlow(param2).Match(
                Some: o => successGenCriter(o),
                None: () => false
                ); ;
    }
    public Option<object[]> GetCurrentPassage()
    {
        throw new NotImplementedException(); // TODO: not done in SGP
        //return xmlRpcRaw.GetCurrentPassage();
    }
    
    // TODO: use case of this function would be different than SGP
    public bool SetOutputClient(int[] param)
    {
        return xmlRpcRaw.SetOutputClient(param).Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }    
    
    public Option<object[]> GetMotorSpeed()
    {
        throw new NotImplementedException(); // TODO: not done in SGP
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

    public bool SetMotorSpeed(MotorSpeed conf)
    {
        object[] param3 = new object[7];
        param3[0] = "Entry";    // TODO: for now, copied from SGP
        param3[1] = conf.StandardOpeningSpeed;
        param3[2] = conf.StandardClosingSpeed;
        param3[3] = conf.SecurityOpeningSpeed;
        param3[4] = conf.DisappearanceSpeed;
        param3[5] = conf.FraudClosingSpeed;
        param3[6] = conf.SecurityFraudClosingSpeed;

        return xmlRpcRaw.SetMotorSpeed(param3).Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }

    public bool SetBuzzerFraud(int volume, int note)
    {
        return xmlRpcRaw.SetBuzzerFraud([volume, note]).Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }

    public bool SetBuzzerIntrusion(int volume, int note)
    {
        return xmlRpcRaw.SetBuzzerIntrusion([volume, note]).Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }

    public bool SetBuzzerMode(int ModeBuzzer)
    {
        return xmlRpcRaw.SetBuzzerMode([ModeBuzzer]).Match(
                Some: o => successGenCriter(o),
                None: () => false
                );
    }
    public record DoorModeConf
        (
        bool ForceDoorinReverseSide,
        bool ForceDoorinEntrySide,
        bool ForceDoorinFreeSide
        );
    public static DoorsMode TransformFromChangeModeDoorsMode(ChangeEquipmentMode EM, DoorModeConf conf)
    {
        DoorsMode _doorsMode = DoorsMode.BlockClosed;
        if (EM.Mode == GlobalEquipmentMode.InService)
        {
            if (EM.Aisle == GlobalEquipmentAisle.NormallyClosed)
            {
                _doorsMode = DoorsMode.Nc;
            }
            else if (EM.Aisle == GlobalEquipmentAisle.NormallyOpen)
            {
                switch (EM.Direction)
                {
                    case GlobalEquipmentDirection.Entry:
                        {
                            switch (EM.EntryMode)
                            {
                                case GlobalEquipmentEntryMode.Controlled:
                                    _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Nob : DoorsMode.Noa;
                                    break;
                                case GlobalEquipmentEntryMode.Free:
                                case GlobalEquipmentEntryMode.FreeControlled:
                                    _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Nob : DoorsMode.Noa;
                                    break;
                            }
                            break;
                        }
                    case GlobalEquipmentDirection.Exit:
                        {
                            switch (EM.ExitMode)
                            {
                                case GlobalEquipmentExitMode.Controlled:
                                    _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Noa : DoorsMode.Nob;
                                    break;
                                case GlobalEquipmentExitMode.Free:
                                case GlobalEquipmentExitMode.FreeControlled:
                                    _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Noa : DoorsMode.Nob;
                                    break;
                            }
                            break;
                        }
                    case GlobalEquipmentDirection.BiDirectional:
                        {
                            switch (EM.EntryMode)
                            {
                                case GlobalEquipmentEntryMode.Controlled:
                                    switch (EM.ExitMode)
                                    {
                                        case GlobalEquipmentExitMode.Controlled:
                                            _doorsMode = conf.ForceDoorinEntrySide ? DoorsMode.Noa : DoorsMode.Nob;
                                            break;
                                        case GlobalEquipmentExitMode.Free:
                                        case GlobalEquipmentExitMode.FreeControlled:
                                            _doorsMode = conf.ForceDoorinFreeSide ? DoorsMode.Nob : DoorsMode.Noa;
                                            break;
                                        case GlobalEquipmentExitMode.Closed:
                                            _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Nob : DoorsMode.Noa;
                                            break;
                                    }
                                    break;
                                case GlobalEquipmentEntryMode.Free:
                                case GlobalEquipmentEntryMode.FreeControlled:
                                    switch (EM.ExitMode)
                                    {
                                        case GlobalEquipmentExitMode.Controlled:
                                            _doorsMode = conf.ForceDoorinFreeSide ? DoorsMode.Noa : DoorsMode.Nob;
                                            break;
                                        case GlobalEquipmentExitMode.Free:
                                        case GlobalEquipmentExitMode.FreeControlled:
                                            _doorsMode = conf.ForceDoorinEntrySide ? DoorsMode.OpticalA : DoorsMode.OpticalB;
                                            break;
                                        case GlobalEquipmentExitMode.Closed:
                                            _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Nob : DoorsMode.Noa;
                                            break;
                                    }
                                    break;
                                case GlobalEquipmentEntryMode.Closed:
                                    switch (EM.ExitMode)
                                    {
                                        case GlobalEquipmentExitMode.Controlled:
                                            _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Noa : DoorsMode.Nob;
                                            break;
                                        case GlobalEquipmentExitMode.Free:
                                        case GlobalEquipmentExitMode.FreeControlled:
                                            _doorsMode = conf.ForceDoorinReverseSide  ? DoorsMode.Noa : DoorsMode.Nob;
                                            break;
                                    }
                                    break;
                            }
                            break;
                        }
                }
            }
        }
        return _doorsMode;
    }
}
