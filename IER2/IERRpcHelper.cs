using EtGate.IER;
using LanguageExt;
using System.Collections;
using System.Xml.Serialization;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager;

public partial class CIERRpcHelper
{
    public CIERRpcHelper(IERXmlRpc _iERXmlRpcInterface)
    {
        this.xmlRpcRaw = _iERXmlRpcInterface;
    }

    private IIerXmlRpc xmlRpcRaw;

    readonly Func<object[], bool> firstElementIsOne = (object[] op) =>
    op.Length > 1
    && int.TryParse(op[0].ToString(), out int res)
    && res == 1;

    public bool SetFirmware(string fileName)
    {
        try
        {
            ////The Gate shall have been previously in OutOfService Mode
            //FileTransfertConfiguration conf = new FileTransfertConfiguration();
            //conf.Password = "upload";
            //conf.Login = "upload";
            //conf.Mode = FileTransfertMode.SftpWinscp;
            //conf.Root = "update";
            //conf.ServerUrl = "192.168.0.200";
            //// TODO: see why its counterpart in SGP too is not compiling because of lack of WinSCP_FileTransfert
            ////WinSCP_FileTransfert fileTransfert = new WinSCP_FileTransfert(conf);
            ////fileTransfert.PutFile(fileName);
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

    public Option<object[]> SetCredentials(string[] param)
    {
        return xmlRpcRaw.SetCredentials(param);
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


    public bool SetBuzzerMode(int ModeBuzzer)
    {
        return xmlRpcRaw.SetBuzzerMode([ModeBuzzer]).Match(
                Some: o => firstElementIsOne(o),
                None: () => false
                );
    }    
}
