using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace IFS2.Equipment.DriverInterface
{
    public enum GlobalEquipmentPaidReader
    {
        Unknown = -1,
        None,
        CSCOnly,
        All,
        QRCodeOnly
    }

    public enum GlobalEquipmentUnpaidReader
    {
        Unknown = -1,
        None,
        CSCOnly,
        All,
        QRCodeOnly
    }

    public enum GlobalEquipmentAisle
    {
        Unknown = -1,
        NormallyClosed,
        NormallyOpen
    }

    public enum ExitTTType
    {
        Unknown = -1,
        None,
        ExitRER,
        ExitMetro,
        MetroToRER,
        RERToMetro,
        MetroToPublic,
        RERToPublic
    }
    public enum EntryTTType
    {
        Unknown = -1,
        None,
        EntryRER,
        EntryMetro,
        MetroToRER,
        RERToMetro,
        PublicToMetro,
        PublicToRER
    }

    public enum GlobalEquipmentDirection
    {
        Unknown = -1,
        None,
        Entry,
        Exit,
        BiDirectional
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

    public enum GlobalEquipmentExitMode
    {
        Unknown = -1,
        None,
        Controlled,
        Free,
        Closed,
        Simulated,
        FreeControlled
    }
    public enum GlobalEquipmentEntryMode
    {
        Unknown = -1,
        None,
        Controlled,
        Free,
        Closed,
        Simulated,
        FreeControlled
    }
    public enum GlobalEquipmentMode
    {
        Unknown = -1,
        InService = 0,
        Maintenance = 1,
        Emergency = 2,
        StationClosed = 3,
        OutOfService = 5,
        OutOfOrder = 6,
        Closed = 7
    }


    [XmlRoot("ChangeEquipmentMode")]
    public class ChangeEquipmentMode
    {
        [XmlElement("Mode")]
        public int _Mode = -1;

        [XmlElement("Direction")]
        public int _Direction = -1;

        [XmlElement("PaidReader")]
        public int _PaidReader = -1;

        [XmlElement("UnpaidReader")]
        public int _UnpaidReader = -1;

        [XmlElement("EntryMode")]
        public int _EntryMode = -1;

        [XmlElement("ExitMode")]
        public int _ExitMode = -1;

        [XmlElement("EntryTTMode")]
        public int _EntryTTType = -1;

        [XmlElement("ExitTTMode")]
        public int _ExitTTType = -1;

        [XmlElement("Aisle")]
        public int _Aisle = -1;

        [XmlElement("Simulated")]
        public int _Simulated = -1;

        [XmlElement("HCEMode")]
        public int _HCEMode = -1;

        [XmlElement("SpecialEvent")]
        public int _SpecialEvent = -1;

        [XmlElement("EntrySpecialEvent")]
        public int _EntrySpecialEvent = 0;

        [XmlElement("ExitSpecialEvent")]
        public int _ExitSpecialEvent = 0;

        [XmlElement("EntryOutOfOrder")]
        public int _EntryOutOfOrder = 0;

        [XmlElement("ExitOutOfOrder")]
        public int _ExitOutOfOrder = 0;

        [XmlElement("Emergency")]
        public int _Emergency = -1;

        [XmlElement("Maintenance")]
        public int _Maintenance = -1;

        [XmlIgnore]
        public GlobalEquipmentMode Mode
        {
            get
            {
                return (GlobalEquipmentMode)_Mode;
            }
            set
            {
                _Mode = (int)value;
            }
        }

        [XmlIgnore]
        public bool Mode_Present => _Mode != -1;

        [XmlIgnore]
        public GlobalEquipmentDirection Direction
        {
            get
            {
                return (GlobalEquipmentDirection)_Direction;
            }
            set
            {
                _Direction = (int)value;
            }
        }

        [XmlIgnore]
        public bool Direction_Present => _Direction != -1;

        [XmlIgnore]
        public GlobalEquipmentPaidReader PaidReader
        {
            get
            {
                return (GlobalEquipmentPaidReader)_PaidReader;
            }
            set
            {
                _PaidReader = (int)value;
            }
        }

        [XmlIgnore]
        public bool PaidReader_Present => _PaidReader != -1;

        [XmlIgnore]
        public GlobalEquipmentUnpaidReader UnpaidReader
        {
            get
            {
                return (GlobalEquipmentUnpaidReader)_UnpaidReader;
            }
            set
            {
                _UnpaidReader = (int)value;
            }
        }

        [XmlIgnore]
        public bool UnpaidReader_Present => _UnpaidReader != -1;

        [XmlIgnore]
        public GlobalEquipmentEntryMode EntryMode
        {
            get
            {
                return (GlobalEquipmentEntryMode)_EntryMode;
            }
            set
            {
                _EntryMode = (int)value;
            }
        }

        [XmlIgnore]
        public bool EntryMode_Present => _EntryMode != -1;

        [XmlIgnore]
        public GlobalEquipmentExitMode ExitMode
        {
            get
            {
                return (GlobalEquipmentExitMode)_ExitMode;
            }
            set
            {
                _ExitMode = (int)value;
            }
        }

        [XmlIgnore]
        public bool ExitMode_Present => _ExitMode != -1;

        [XmlIgnore]
        public EntryTTType EntryTTType
        {
            get
            {
                return (EntryTTType)_EntryTTType;
            }
            set
            {
                _EntryTTType = (int)value;
            }
        }

        [XmlIgnore]
        public bool EntryTTType_Present => _EntryTTType != -1;

        [XmlIgnore]
        public ExitTTType ExitTTType
        {
            get
            {
                return (ExitTTType)_ExitTTType;
            }
            set
            {
                _ExitTTType = (int)value;
            }
        }

        [XmlIgnore]
        public bool ExitTTType_Present => _ExitTTType != -1;

        [XmlIgnore]
        public GlobalEquipmentAisle Aisle
        {
            get
            {
                return (GlobalEquipmentAisle)_Aisle;
            }
            set
            {
                _Aisle = (int)value;
            }
        }

        [XmlIgnore]
        public bool Aisle_Present => _Aisle != -1;

        [XmlIgnore]
        public bool Simulated
        {
            get
            {
                return _Simulated == 1;
            }
            set
            {
                _Simulated = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool Simulated_Present => _Simulated != -1;

        [XmlIgnore]
        public bool HCEMode
        {
            get
            {
                return _HCEMode == 1;
            }
            set
            {
                _HCEMode = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool HCEMode_Present => _HCEMode != -1;

        [XmlIgnore]
        public bool SpecialEvent
        {
            get
            {
                return _SpecialEvent == 1;
            }
            set
            {
                _SpecialEvent = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool SpecialEvent_Present => _SpecialEvent != -1;

        [XmlIgnore]
        public bool EntrySpecialEvent
        {
            get
            {
                return _EntrySpecialEvent == 1;
            }
            set
            {
                _EntrySpecialEvent = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool EntrySpecialEvent_Present => _EntrySpecialEvent != -1;

        [XmlIgnore]
        public bool ExitSpecialEvent
        {
            get
            {
                return _ExitSpecialEvent == 1;
            }
            set
            {
                _ExitSpecialEvent = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool ExitSpecialEvent_Present => _ExitSpecialEvent != -1;

        [XmlIgnore]
        public bool EntryOutOfOrder
        {
            get
            {
                return _EntryOutOfOrder == 1;
            }
            set
            {
                _EntryOutOfOrder = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool EntryOutOfOrder_Present => _EntryOutOfOrder != -1;

        [XmlIgnore]
        public bool ExitOutOfOrder
        {
            get
            {
                return _ExitOutOfOrder == 1;
            }
            set
            {
                _ExitOutOfOrder = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool ExitOutOfOrder_Present => _ExitOutOfOrder != -1;

        [XmlIgnore]
        public bool Emergency
        {
            get
            {
                return _Emergency == 1;
            }
            set
            {
                _Emergency = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool Emergency_Present => _Emergency != -1;

        [XmlIgnore]
        public bool Maintenance
        {
            get
            {
                return _Maintenance == 1;
            }
            set
            {
                _Maintenance = (value ? 1 : 0);
            }
        }

        [XmlIgnore]
        public bool Maintenance_Present => _Maintenance != -1;

        public ChangeEquipmentMode(GlobalEquipmentMode Mode_)
        {
            Mode = Mode_;
        }

        public ChangeEquipmentMode(GlobalEquipmentDirection Direction_)
        {
            Direction = Direction_;
        }

        public ChangeEquipmentMode(bool Simulated_)
        {
            _Simulated = (Simulated_ ? 1 : 0);
        }

        public ChangeEquipmentMode(ChangeEquipmentMode init)
        {
            Mode = init.Mode;
            Direction = init.Direction;
            PaidReader = init.PaidReader;
            UnpaidReader = init.UnpaidReader;
            EntryMode = init.EntryMode;
            ExitMode = init.ExitMode;
            EntryTTType = init.EntryTTType;
            ExitTTType = init.ExitTTType;
            Aisle = init.Aisle;
            Simulated = init.Simulated;
            HCEMode = init.HCEMode;
            SpecialEvent = init.SpecialEvent;
            EntrySpecialEvent = init.EntrySpecialEvent;
            ExitSpecialEvent = init.ExitSpecialEvent;
            EntryOutOfOrder = init.EntryOutOfOrder;
            ExitOutOfOrder = init.ExitOutOfOrder;
            Emergency = init.Emergency;
            Maintenance = init.Maintenance;
        }

        public ChangeEquipmentMode()
        {
        }

        public string ToXMLString()
        {
            throw new NotImplementedException();
        }

        public static ChangeEquipmentMode FromXMLString(string data)
        {
            throw new NotImplementedException();
        }
    }
    

    [XmlRoot("Data")]
    public class IERPLCConfigurationData2
    {
        [XmlElement("ModeBuzzer")]
        [JsonInclude]
        [JsonPropertyName("ModeBuzzer")]
        public int ModeBuzzer = 0;

        [XmlElement("BuzzerIntrusionVolume")]
        [JsonInclude]
        [JsonPropertyName("BuzzerIntrusionVolume")]
        public int BuzzerIntrusionVolume = 0;

        [XmlElement("BuzzerIntrusionNote")]
        [JsonInclude]
        [JsonPropertyName("BuzzerIntrusionNote")]
        public int BuzzerIntrusionNote = 0;

        [XmlElement("BuzzerFraudVolume")]
        [JsonInclude]
        [JsonPropertyName("BuzzerFraudVolume")]
        public int BuzzerFraudVolume = 0;

        [XmlElement("BuzzerFraudNote")]
        [JsonInclude]
        [JsonPropertyName("BuzzerFraudNote")]
        public int BuzzerFraudNote = 0;

        public IERPLCConfigurationData2(IERPLCConfigurationData2 init)
        {
            ModeBuzzer = init.ModeBuzzer;
            BuzzerIntrusionVolume = init.BuzzerIntrusionVolume;
            BuzzerIntrusionNote = init.BuzzerIntrusionNote;
            BuzzerFraudVolume = init.BuzzerFraudVolume;
            BuzzerFraudNote = init.BuzzerFraudNote;
        }

        public IERPLCConfigurationData2()
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

        public static IERPLCConfigurationData2 FromXMLString(string data)
        {
            throw new NotImplementedException();
        }

        public static IERPLCConfigurationData2 FromJSONString(string data)
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

    public class CPLCStatus
    {

        public int[] Authorizations = new int[2];///nb Authorizations [0]=>Entry or A , [1] Exit or B
        //public DoorsMode door_mode;
        public eDoorCurrentState eDoorState;
        //public ePLCState eplcState;
        public GlobalEquipmentMode Mode; //Global equipment mode.
        public GlobalEquipmentEntryMode EntryMode;
        public GlobalEquipmentExitMode ExitMode;
        public GlobalEquipmentDirection Direction;
        public GlobalEquipmentAisle Aisle;
        public bool bDoorFailure;//Indication if there is a failure on obstacle. true=failure. Set to true also if mode is Technical Failure
        public bool bDoorsBlocked;//0
        public bool bDoorForced;//Set to true when mode is locked-open ??
        public bool bSafetyZoneActivated;//Indication if there is a passenger in safety zone. true=presence
        public bool bIntrusion; //true if door state indicates Intrusion


        public bool bPasageCancelled;
        public bool bPasageSuccess;
        public bool bPassageClearTimeout;
        public bool bUnAuthrizedPassage;
        //public eSideOperatingModeGate sideOperatingModeA, sideOperatingModeB;
        public int[] people_detected = new int[2]; //nb people detected [0]=>Entry or A , [1] Exit or B
        public int[] presence = new int[2];/// , people_presence_B;// people presence detected
        public bool bDoorsBlockedClosed; //true is door indicated blocked closed. To see usage.
        public bool bModeStatus;

        //Alarms
        public bool bWWIntrusionEntry = false;
        public bool bWWIntrusionExit = false;
        public bool bFraudEntry=false; 
        public bool bFraudExit=false; 
        public bool bFraudJump = false; 
        public bool bFraudRamp = false;
        public bool bFraudOthers = false;
        public bool bFraud=false;
        public bool bWWIntrusion=false;
        public bool bPreIntrusionEntry = false;
        public bool bPreIntrusionExit = false;
        public bool bPreIntrusion = false;

        public bool bPoweredOnUPS = false;
        public bool bEntryMaintenanceSwitch = false;
        public bool bExitMaintenanceSwitch = false;
        public bool bEmergencyButton = false;

        //Failures
        public EventAlarmLevel CanBus=EventAlarmLevel.Alarm;
        public string CanBusDetail = "";
        public EventAlarmLevel Camera=EventAlarmLevel.Alarm;
        public string CameraDetail = "";
        public EventAlarmLevel Motor = EventAlarmLevel.Alarm;
        public string MotorDetail = "";
        public EventAlarmLevel Sensors = EventAlarmLevel.Alarm;
        public string SensorsDetail = "";
        public EventAlarmLevel Brake = EventAlarmLevel.Alarm;
        public string BrakeDetail = "";
        public EventAlarmLevel GCUTemperature = EventAlarmLevel.Alarm;
        public string GCUTemperatureDetail = "";
        public EventAlarmLevel GCUElectronic = EventAlarmLevel.Alarm;
        public string GCUElectronicDetail = "";
        public EventAlarmLevel AFCCommunication = EventAlarmLevel.Alarm;
        public string AFCCommunicationDetail = "";
        public EventAlarmLevel GCUSystem = EventAlarmLevel.Alarm;
        public string GCUSystemDetail = "";
        /*
         * infractions/Alarm currently in the gates its stored in bits 
         * b[0] = An unauthorised person has crossed the gate from the entrance (A side)
         * b[1] =  An unauthorised person has crossed the gate from the exit (B side) 
         * b[2] = A person has disappeared from the gate
         * b[3] = Fraud holding
         * b[4] = Somebody has jumped over the obstacles
         * b[5] = Somebody is crawling under the obstacles
         * b[6] = Unexpected movement fraud
         * b[7] = An unauthorised person is standing at the entrance while the gate is idle or during a passage from the entry
         * b[8] = An unauthorised person is standing at the exit while the gate is idle or during a passage from the exit
         * b[9] = An unauthorised person is standing at the entrance during a passage from the exit (WWI)
         * b[10] = An unauthorised person is standing at the exit during a passage from the entrance (WWI)
         * b[11] = An unauthorised person is standing at the entrance when the doors are closed
         * b[12] = An unauthorised person is standing at exit when the doors are closed
         * b[13] = Fraud in Mantrap
         */
        public Int16 infractions; //b0 = 
        //private string[] Alarms;
        //public string[] failures_major;
        //public Dictionary<string, string[]> failures;
        private Dictionary<string, int> inputs;
        public Dictionary<string, object> rawData;
        public STATUS callreturnStatus = STATUS.NOK;
        public int bInfractionsDirection = -1;
       

        public CPLCStatus()
        {
            callreturnStatus = STATUS.NOK;
            //door_mode = DoorsMode.NONE;
            eDoorState = eDoorCurrentState.NONE;
            //eplcState = ePLCState.ERROR_INIT;
            Mode=GlobalEquipmentMode.OutOfService;
            EntryMode = GlobalEquipmentEntryMode.Closed;
            ExitMode=GlobalEquipmentExitMode.Closed ;
            Direction=GlobalEquipmentDirection.None;
            Aisle = GlobalEquipmentAisle.NormallyClosed; 
            bDoorFailure = false;
            bSafetyZoneActivated = false;
            bIntrusion = false;
            bDoorsBlocked = false;
            bDoorForced = false;
            bPasageSuccess = false;
            bPasageCancelled = false;

            bInfractionsDirection = -1;

            //Indicators from failure part

        }
        public void Reset()
        {
            this.callreturnStatus = STATUS.NOK;
            //this.door_mode = DoorsMode.NONE;
            this.eDoorState = eDoorCurrentState.NONE;
            //this.eplcState = ePLCState.ERROR_INIT;
            Mode = GlobalEquipmentMode.OutOfService;
            EntryMode = GlobalEquipmentEntryMode.Closed;
            ExitMode = GlobalEquipmentExitMode.Closed;
            Direction = GlobalEquipmentDirection.None;
            Aisle = GlobalEquipmentAisle.NormallyClosed;
            this.bDoorFailure = false;
            this.bSafetyZoneActivated = false;
            this.bIntrusion = false;

            this.bDoorsBlocked = false;
            this.bDoorForced = false;
            this.bDoorsBlockedClosed = false;
            //bInfractionsDirection = -1;
        }

        public void Clear()
        {
            bDoorFailure = false;
            bSafetyZoneActivated = false;
            bIntrusion = false;

            bDoorsBlocked = false;
            bDoorForced = false;
            bDoorsBlockedClosed = false;
            //bInfractionsDirection = -1;
        }
    }

    //public class CPLCConfig
    //{
    //    public int FlapRemainOpen_PostPasses=-1;//Time the obstacles remain open after a user leaves the lane(in Free Mode).
    //    public int EntranceTime = -1;// Time allotted to enter in the lane after an authorisation is granted
    //    public int BadgeTime = -1;//Time allotted to badge after a user is detected in the lane (in Controlled Mode only)
    //    public int TimeToCross_afterDetection = -1;//Time allotted to completely cross the lane after a person is detected.
    //    public int ExitSafetyZone = -1;//Time allotted to exit the safety zone.
    //    public int CompletePassesTime = -1;//Time allotted to completely cross the lane after an authorisation is granted
    //}

    public enum ePLCErrors
    {
        NONE = 0,
        ERRORS_DOORS_BLOCKED,
        ERRORS_DOORS_FORCED,
        ERRORS_MOTOR_INIT,
        ERRORS_DOORS_COMM,
        ERRORS_SENSORS, //ERRORS_DETECTION'      
        ERRORS_SW,

    }
    public enum STATUS
    {

        OK = 0,
        NOK = -1,
        GCU_COMM_ERROR = -2,
        GCU_NOT_INIT = -4
    }
    //Side Operating Modes
    public enum SideOperatingModes
    {
        Closed,
        Controlled,
        Free,
        None
    }
    public enum eSideOperatingModeGate
    {
        OP_MODE_SIDE_CLOSED,
        OP_MODE_SIDE_CONTROLLED,
        OP_MODE_SIDE_FREE,
        //OP_MODE_SIDE_FREECONTROLLED
    }
    public enum DoorsMode
    {
        NONE = -1,
        BlockClosed, /*Locked CLosed*/
        Nc,  /*Normally Closed*/
        Noa, /*Normally Opened A*/
        Nob, /*Normally Opened B*/
        OpticalA, /*Optical Mode A*/
        OpticalB, /*Optical Mode B*/
        LockedOpenA, /*Blocked Open entry */
        LockedOpenB /*Blocked Open exit */   
    };

    public enum eDoorCurrentState //Flaps current state
    {
        NONE = -1,
        DOOR_OPENED,
        DOOR_CLOSED,
        DOOR_MOVING,
        FRAUD
    }
    public enum ePLCState //PLC state Machine
    {
        ERROR_INIT = -1,
        INSERVICE = 0, 
        MAINTENANCE = 1,
        EMERGENCY = 2,
        OUTOFSERVICE = 5,//flaps forced or blocked ? value of OUTOFSERVICE=5 added by Vikash on 27-02-2023
        OUTOFORDER=6,
        FAILURE = 7
    }
    public class PLCHardwareInterfaceModel //: HardwareDriverModel, HardwareDriverInterface
    {
        public virtual bool OnBegin()
        {
            return true;
        }

        public virtual STATUS Reboot(bool bhardboot)
        {
            return STATUS.OK;
        }

        public virtual bool ReInitialize()
        {
            return true;
        }

        public virtual CPLCStatus GetStatus()
        {
            return new CPLCStatus();
        }

        public virtual bool GetFirmwareVersion(out string firmware)
        {
            firmware = "XXX";
            return true;
        }

        public virtual bool GetLibraryVersion(out string libraryVersion)
        {
            libraryVersion = "XXX";
            return true;
        }

        public virtual STATUS AuthorizePassage(int nbpassage, int direction)
        {
            return STATUS.OK;
        }

        public virtual STATUS SetFlapOperationlMode(string FlapsMode, string OperatingModes_EntrySide, string OperatingModes_ExitSide)
        {
            return STATUS.OK;
        }

        public virtual STATUS SetEquipmentMode(ChangeEquipmentMode EM)
        {
            return STATUS.OK;
        }

        public virtual STATUS SetMaintenanceMode(bool onOff)
        {
            return STATUS.OK;
        }

        public virtual STATUS SetEmergencyMode(bool onOff)
        {
            return STATUS.OK;
        }

        public virtual STATUS SetCredentials(string userName, string Password)
        {
            return STATUS.OK;
        }

        public virtual STATUS SetDateTime(DateTime dt)
        {
            return STATUS.OK;
        }

        public virtual STATUS GetDateTime(out DateTime dt)
        {
            dt = DateTime.Now;
            return STATUS.OK;
        }

        public virtual STATUS ConfigurePassage(IERPLCConfigurationData conf, IERPLCConfigurationData2 confInternal)
        {
            return STATUS.OK;
        }
        public virtual string MethodHelp(string MethodName)
        {
            return string.Empty;
        }
        public virtual STATUS ManagePassageLight(int EntryReader, int ExitReader)//On=1, off=0
        {
            return STATUS.OK;
        }

        public virtual STATUS SetFirmware(string fileName)
        {
            return STATUS.OK;
        }

        public virtual Dictionary<int, int> GetCounters(int type)
        {
            return new Dictionary<int, int>();  
        }
    }
}
