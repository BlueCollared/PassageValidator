using IFS2.Common;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace IFS2.Equipment.DriverInterface
{

    

    //public enum GlobalEquipmentMode
    //{
    //    Unknown = -1,
    //    InService = 0,
    //    Maintenance = 1,
    //    Emergency = 2,
    //    StationClosed = 3,
    //    OutOfService = 5,
    //    OutOfOrder = 6,
    //    Closed = 7
    //}


    
    public class ChangeEquipmentMode
    {        
        public GlobalEquipmentMode Mode { get; set; }
        public GlobalEquipmentDirection Direction { get; set; }        
        public GlobalEquipmentPaidReader PaidReader { get; set; }        
        public GlobalEquipmentUnpaidReader UnpaidReader { get; set; }
        public GlobalEquipmentEntryMode EntryMode { get; set; }        
        public GlobalEquipmentExitMode ExitMode { get; set; }        
        public EntryTTType EntryTTType { get; set; }
        public ExitTTType ExitTTType { get; set; }
        public GlobalEquipmentAisle Aisle { get; set; }
        public bool Simulated { get; set; }
        public bool EntryOutOfOrder { get; set; }
        public bool ExitOutOfOrder { get; set; }
        public bool Emergency { get; set; }        
        public bool Maintenance { get; set; }

        public ChangeEquipmentMode(GlobalEquipmentMode Mode_)
        {
            Mode = Mode_;
        }

        public ChangeEquipmentMode(GlobalEquipmentDirection Direction_)
        {
            Direction = Direction_;
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
        
        public int bInfractionsDirection = -1;
       

        public CPLCStatus()
        {            
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
}
