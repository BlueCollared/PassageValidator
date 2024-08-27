using EtGate.IER;
using IFS2.Common;
using IFS2.Common.CoreTechnical;
using IFS2.Equipment.DriverInterface;
using LanguageExt;
using System.Collections;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager;

public class CIERRpcHelper
{
    public CIERRpcHelper(IERXmlRpcRaw _iERXmlRpcInterface)
    {
        this.xmlRpcRaw = _iERXmlRpcInterface;
    }

    private IIERXmlRpcRaw xmlRpcRaw;

    public static int ResponseParser(object obj, ref Dictionary<string, object> dict)
    {
        // Dictionary<string, object> newDict = new Dictionary<string, object>();
        if (typeof(IDictionary).IsAssignableFrom(obj.GetType()))
         {
            IDictionary idict = (IDictionary)obj;

            foreach (object key in idict.Keys)
            {
                if (typeof(IDictionary).IsAssignableFrom(idict[key].GetType()))
                {
                    // Console.WriteLine("Recursion for Dict Key  Name: " + objToString(key));

                    Dictionary<string, object> temp = new Dictionary<string, object>();
                    if (ResponseParser(idict[key], ref temp) > 0)
                        dict.Add(objToString(key), temp);
                    //newDict.Clear();
                }
                //if (idict[key].GetType().FullName != "Horizon.XmlRpc.Core.XmlRpcStruct")
                else
                {
                    dict.Add(objToString(key), idict[key]);
                }
            }
        }
        else
        {
            // My object is not a dictionary
        }
        return dict.Count;
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

    // object ();

    public object GetStatusEx()
    {
        return xmlRpcRaw.GetStatusEx();
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
                case DriverInterface.GlobalEquipmentEntryMode.Controlled:
                    if (EM.Direction == DriverInterface.GlobalEquipmentDirection.Entry || EM.Direction == DriverInterface.GlobalEquipmentDirection.BiDirectional)
                        _SideOperationModeA = SideOperatingModes.Controlled;
                    break;
                case DriverInterface.GlobalEquipmentEntryMode.Free:
                case DriverInterface.GlobalEquipmentEntryMode.FreeControlled:
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
                case DriverInterface.GlobalEquipmentExitMode.Controlled:
                    if (EM.Direction == DriverInterface.GlobalEquipmentDirection.Exit || EM.Direction == DriverInterface.GlobalEquipmentDirection.BiDirectional)
                        _SideOperationModeB = SideOperatingModes.Controlled;
                    break;
                case DriverInterface.GlobalEquipmentExitMode.Free:
                case DriverInterface.GlobalEquipmentExitMode.FreeControlled:
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
        return xmlRpcRaw.GetCurrentPassage();
    }
    public Option<object[]> SetOutputClient(int[] param)
    {
        return xmlRpcRaw.SetOutputClient(param);
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
                    case DriverInterface.GlobalEquipmentDirection.Entry:
                        {
                            switch (EM.EntryMode)
                            {
                                case DriverInterface.GlobalEquipmentEntryMode.Controlled:
                                    _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Nob : DoorsMode.Noa;
                                    break;
                                case DriverInterface.GlobalEquipmentEntryMode.Free:
                                case DriverInterface.GlobalEquipmentEntryMode.FreeControlled:
                                    _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Nob : DoorsMode.Noa;
                                    break;
                            }
                            break;
                        }
                    case DriverInterface.GlobalEquipmentDirection.Exit:
                        {
                            switch (EM.ExitMode)
                            {
                                case DriverInterface.GlobalEquipmentExitMode.Controlled:
                                    _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Noa : DoorsMode.Nob;
                                    break;
                                case DriverInterface.GlobalEquipmentExitMode.Free:
                                case DriverInterface.GlobalEquipmentExitMode.FreeControlled:
                                    _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Noa : DoorsMode.Nob;
                                    break;
                            }
                            break;
                        }
                    case DriverInterface.GlobalEquipmentDirection.BiDirectional:
                        {
                            switch (EM.EntryMode)
                            {
                                case DriverInterface.GlobalEquipmentEntryMode.Controlled:
                                    switch (EM.ExitMode)
                                    {
                                        case DriverInterface.GlobalEquipmentExitMode.Controlled:
                                            _doorsMode = conf.ForceDoorinEntrySide ? DoorsMode.Noa : DoorsMode.Nob;
                                            break;
                                        case DriverInterface.GlobalEquipmentExitMode.Free:
                                        case DriverInterface.GlobalEquipmentExitMode.FreeControlled:
                                            _doorsMode = conf.ForceDoorinFreeSide ? DoorsMode.Nob : DoorsMode.Noa;
                                            break;
                                        case DriverInterface.GlobalEquipmentExitMode.Closed:
                                            _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Nob : DoorsMode.Noa;
                                            break;
                                    }
                                    break;
                                case DriverInterface.GlobalEquipmentEntryMode.Free:
                                case DriverInterface.GlobalEquipmentEntryMode.FreeControlled:
                                    switch (EM.ExitMode)
                                    {
                                        case DriverInterface.GlobalEquipmentExitMode.Controlled:
                                            _doorsMode = conf.ForceDoorinFreeSide ? DoorsMode.Noa : DoorsMode.Nob;
                                            break;
                                        case DriverInterface.GlobalEquipmentExitMode.Free:
                                        case DriverInterface.GlobalEquipmentExitMode.FreeControlled:
                                            _doorsMode = conf.ForceDoorinEntrySide ? DoorsMode.OpticalA : DoorsMode.OpticalB;
                                            break;
                                        case DriverInterface.GlobalEquipmentExitMode.Closed:
                                            _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Nob : DoorsMode.Noa;
                                            break;
                                    }
                                    break;
                                case DriverInterface.GlobalEquipmentEntryMode.Closed:
                                    switch (EM.ExitMode)
                                    {
                                        case DriverInterface.GlobalEquipmentExitMode.Controlled:
                                            _doorsMode = conf.ForceDoorinReverseSide ? DoorsMode.Noa : DoorsMode.Nob;
                                            break;
                                        case DriverInterface.GlobalEquipmentExitMode.Free:
                                        case DriverInterface.GlobalEquipmentExitMode.FreeControlled:
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
