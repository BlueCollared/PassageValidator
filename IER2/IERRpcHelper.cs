using EtGate.IER;
using IFS2.Common;
using IFS2.Common.CoreTechnical;
using IFS2.Equipment.DriverInterface;
using LanguageExt;
using System.Collections;
using System.Xml.Serialization;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager;

public partial class CIERRpcHelper
{
    public CIERRpcHelper(IERXmlRpcRaw _iERXmlRpcInterface)
    {
        this.xmlRpcRaw = _iERXmlRpcInterface;
    }

    private IIERXmlRpcRaw xmlRpcRaw;

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
            .Match(Some: o => firstElementIsOne(o),
                None: () => false
                );
    }
    
    public bool SetEmergency(bool onOff)
    {
        int[] paramGateMode = { 0 };
        paramGateMode[0] = onOff ? 1 : 0;
        return xmlRpcRaw.SetEmergency(paramGateMode)
            .Match(Some: o => firstElementIsOne(o),
                None: () => false
                );
    }
    
    public bool SetMaintenanceMode(bool onOff)
    {
        int[] paramGateMode = { 0 };
        paramGateMode[0] = onOff ? 1 : 0;

        return xmlRpcRaw.SetMaintenanceMode(paramGateMode)
            .Match(Some: o => firstElementIsOne(o),
                None: () => false
                );
    }

    readonly Func<object[], bool> firstElementIsOne = (object[] op) =>
    op.Length > 1
    && int.TryParse(op[0].ToString(), out int res)
    && res == 1;


    public bool Reboot(bool bhardboot)
    {        
        if (bhardboot)
            return xmlRpcRaw.Reboot().Match(
                Some: o => firstElementIsOne(o),
                None: () => false
                );
        else
            return xmlRpcRaw.Restart().Match(
                Some: o => firstElementIsOne(o),
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
                Some: o => firstElementIsOne(o),
                None: () => false
                );
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

    public Option<CPLCStatus> GetStatusEx()
    {
        var extractor = (object result) => {
            try
            {
                CPLCStatus cPLCStatus = new();
                cPLCStatus.Reset();

                cPLCStatus.rawData = IERRHelper.ResponseParser(result);
                if (cPLCStatus.rawData.Count > 0)
                    IERRHelper.DecodeStatusData(cPLCStatus);
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
                Some: o => firstElementIsOne(o),
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
        return SetDoorlMode(IERRHelper.TransformFromChangeModeDoorsMode(EM, doorMode).ToString(), IERRHelper.TransformFromChangeModeSideA(EM).ToString(), IERRHelper.TransformFromChangeModeSideB(EM).ToString());
    }

    bool SetDoorlMode(string FlapsMode, string OperatingModes_EntrySide, string OperatingModes_ExitSide)
    {
        string[] paramGateMode = new string[3];
        paramGateMode[0] = FlapsMode;
        paramGateMode[1] = OperatingModes_EntrySide;
        paramGateMode[2] = OperatingModes_ExitSide;

        return xmlRpcRaw.SetMode(paramGateMode)
            .Match(
                Some: o => firstElementIsOne(o),
                None: () => false
                );
    }

    public Option<object[]> SetCredentials(string[] param)
    {
        return xmlRpcRaw.SetCredentials(param);
    }

    

    public bool GetSetTempoFlow(TempoFlowConf conf)
    {
        int[] param2 = new int[2];
        param2[0] = conf.FlapRemainOpenPostFreePasses; //Time the obstacles remain open after a user leaves the lane(in Free Mode).
        param2[1] = conf.FlapRemainOpenPostControlledPasses;  //Time the obstacles remain open after a user leaves the lane(Controlled Mode).

        return xmlRpcRaw.GetSetTempoFlow(param2).Match(
                Some: o => firstElementIsOne(o),
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
                Some: o => firstElementIsOne(o),
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
                Some: o => firstElementIsOne(o),
                None: () => false
                );
    }

    public bool SetBuzzerFraud(int volume, int note)
    {
        return xmlRpcRaw.SetBuzzerFraud([volume, note]).Match(
                Some: o => firstElementIsOne(o),
                None: () => false
                );
    }

    public bool SetBuzzerIntrusion(int volume, int note)
    {
        return xmlRpcRaw.SetBuzzerIntrusion([volume, note]).Match(
                Some: o => firstElementIsOne(o),
                None: () => false
                );
    }

    public bool SetBuzzerMode(int ModeBuzzer)
    {
        return xmlRpcRaw.SetBuzzerMode([ModeBuzzer]).Match(
                Some: o => firstElementIsOne(o),
                None: () => false
                );
    }    
}
