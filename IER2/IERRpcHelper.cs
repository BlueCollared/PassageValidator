using EtGate.IER;
using Horizon.XmlRpc.Client;
using IFS2.Common;
using IFS2.Common.CoreTechnical;
using IFS2.Equipment.DriverInterface;
using LanguageExt;
using System;
using System.Collections;
using System.Net;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager;

public class CIERRpcHelper
{
    public CIERRpcHelper(IERXmlRpcRaw _iERXmlRpcInterface)
    {
        this.xmlRpcRaw = _iERXmlRpcInterface;
        //_iERXmlRpcInterface = XmlRpcProxyGen.Create<IIERXmlRpcInterface>();

        //  this.url = url;
        //this.port = port;
    }

    //private IIERXmlRpcInterface _iERXmlRpcInterface; // TODO: if required, inject it rather than creating it here
    private IIERXmlRpcRaw xmlRpcRaw;


    //private readonly string url;
    //private readonly string port;        


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

    public Option<object[]> SetAuthorisation(int[] param)
    {
        return xmlRpcRaw.SetAuthorisation(param);
    }
    
    public Option<object[]> SetEmergency(int[] param)
    {
        return xmlRpcRaw.SetEmergency(param);
    }
    
    public Option<object[]> SetMaintenanceMode(int[] param)
    {
        return xmlRpcRaw.SetMaintenanceMode(param);
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

    public Option<object[]> SetDate(DateTime dt, string timezone = "")
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
        return xmlRpcRaw.SetDate(dtparams);
    }

    public Option<object[]> GetDate()
    {
        return xmlRpcRaw.GetDate();
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
public Option<object[]> SetMode(string[] param)
    {
        return xmlRpcRaw.SetMode(param);
    }
    public Option<object[]> SetCredentials(string[] param)
    {
        return xmlRpcRaw.SetCredentials(param);
    }
    public Option<object[]> GetSetTempo(int[] param)
    {
        return xmlRpcRaw.GetSetTempo(param);
    }
    public Option<object[]> GetSetTempoFlow(int[] param)
    {
        return xmlRpcRaw.GetSetTempoFlow(param);
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
        return xmlRpcRaw.GetMotorSpeed();
    }
    public Option<object[]> SetMotorSpeed(object[] param)
    {
        return xmlRpcRaw.SetMotorSpeed(param);
    }

    public Option<object[]> SetBuzzerFraud(int[] param)
    {
        return xmlRpcRaw.SetBuzzerFraud(param);
    }

    public Option<object[]> SetBuzzerIntrusion(int[] param)
    {
        return xmlRpcRaw.SetBuzzerIntrusion(param);
    }

    public Option<object[]> SetBuzzerMode(int[] param)
    {
        return xmlRpcRaw.SetBuzzerMode(param);
    }
}
