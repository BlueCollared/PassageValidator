using EtGate.IER;
using Horizon.XmlRpc.Client;
using LanguageExt;
using System.Collections;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager
{
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

        public object[] SetAuthorisation(int[] param)
        {
            return xmlRpcRaw.SetAuthorisation(param);
        }
        
        public object[] SetEmergency(int[] param)
        {
            return xmlRpcRaw.SetEmergency(param);
        }
        
        public object[] SetMaintenanceMode(int[] param)
        {
            return xmlRpcRaw.SetMaintenanceMode(param);
        }
        
        public bool Reboot(bool bhardboot)
        {
            var bootSuccess = (object[] op) => 
                op.Length > 1
                && int.TryParse(op[0].ToString(), out int res)
                && res > 0;
            
            if (bhardboot)
                return xmlRpcRaw.Reboot().Match(
                    Some: o => bootSuccess(o),
                    None: () => false
                    );
            else
                return xmlRpcRaw.Restart().Match(
                    Some: o => bootSuccess(o),
                    None: () => false
                    );
        }

        public object[] ApplyUpdate()
        {
            return xmlRpcRaw.ApplyUpdate();
        }

        public object[] SetDate(DateTime dt, string timezone = "")
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

        public object[] GetDate()
        {
            return xmlRpcRaw.GetDate();
        }

        // object ();

        public object GetStatusEx()
        {
            return xmlRpcRaw.GetStatusEx();
        }

        public IERSWVersion GetVersion()
        {
            IERSWVersion iERSW = new();
            object[] ret = xmlRpcRaw.GetVersion();
            if (ret.Length > 0)
            {
                iERSW.LaneType = (string)ret[0];
                iERSW.SWVersion = (string)ret[1];
                iERSW.CompilationDate = (string)ret[2];
                iERSW.GITVersion = (string)ret[3];
                iERSW.GITDate = (string)ret[4];
            }
            return iERSW;
        }

        public Dictionary<int, int> GetCounter()
        {
            Dictionary<int, int> counters = new();
            object[] result = xmlRpcRaw.GetCounter();
            int nb;
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
        }

            public object[] SetMode(string[] param)
        {
            return xmlRpcRaw.SetMode(param);
        }
        public object[] SetCredentials(string[] param)
        {
            return xmlRpcRaw.SetCredentials(param);
        }
        public object[] GetSetTempo(int[] param)
        {
            return xmlRpcRaw.GetSetTempo(param);
        }
        public object[] GetSetTempoFlow(int[] param)
        {
            return xmlRpcRaw.GetSetTempoFlow(param);
        }
        public object[] GetCurrentPassage()
        {
            return xmlRpcRaw.GetCurrentPassage();
        }
        public object[] SetOutputClient(int[] param)
        {
            return xmlRpcRaw.SetOutputClient(param);
        }
        public object[] GetMotorSpeed()
        {
            return xmlRpcRaw.GetMotorSpeed();
        }
        public object[] SetMotorSpeed(object[] param)
        {
            return xmlRpcRaw.SetMotorSpeed(param);
        }

        public object[] SetBuzzerFraud(int[] param)
        {
            return xmlRpcRaw.SetBuzzerFraud(param);
        }

        public object[] SetBuzzerIntrusion(int[] param)
        {
            return xmlRpcRaw.SetBuzzerIntrusion(param);
        }

        public object[] SetBuzzerMode(int[] param)
        {
            return xmlRpcRaw.SetBuzzerMode(param);
        }
    }
}
