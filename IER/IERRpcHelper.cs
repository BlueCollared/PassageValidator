using Horizon.XmlRpc.Client;
//using IFS2.Common.CoreTechnical;
using System.Collections;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager
{
    public class CIERRpcHelper
    {
        private static IIERXmlRpcInterface _iERXmlRpcInterface = null;
        private static bool _bisInitialized = false;
        //private static string _port, _url;


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

        public static bool InitializeRpc(string url, string port)
        {
            try
            {
                if (_iERXmlRpcInterface != null || _bisInitialized == true)
                {
                    _iERXmlRpcInterface = null;
                    _bisInitialized = false;
                }
                //_port = port;
                //_url = url;
                _iERXmlRpcInterface = XmlRpcProxyGen.Create<IIERXmlRpcInterface>();
                _bisInitialized = true;
                //_iERXmlRpcInterface.Url = _url + ":" + _port.ToString();
            }
            catch (Exception e)
            {
                _bisInitialized = false;
            }
            return _bisInitialized;

        }

        //public int SendCommad(string fun, object param);
        public static string[] ListAvailableMethods()
        {
            return _iERXmlRpcInterface.SystemListMethods();
        }

        public static object[] SetAuthorization(int[] param)
        {
            return _iERXmlRpcInterface.SetAuthorisation(param);
        }

        //SetEmergency
        public static object[] SetEmergency(int[] param)
        {
            return _iERXmlRpcInterface.SetEmergency(param);
        }

        //SetMaintenanceMode
        public static object[] SetMaintenanceMode(int[] param)
        {
            return _iERXmlRpcInterface.SetMaintenanceMode(param);
        }

        //SendReboot
        public static object[] Reboot(bool bhardboot)
        {
            if (bhardboot) return _iERXmlRpcInterface.SendReboot();
            else return _iERXmlRpcInterface.SendRestart();
        }

        public static object[] ApplyUpdate()
        {
            return _iERXmlRpcInterface.ApplyUpdate();
        }

        public static object[] SetDate(DateTime dt, string timezone = "")
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
            return _iERXmlRpcInterface.SetDate(dtparams);
        }

        public static object[] GetDate()
        {
            return _iERXmlRpcInterface.GetDate();
        }

        // object ();

        public static object GetStatusEx()
        {
            return _iERXmlRpcInterface.GetStatusStd();
        }

        public static IERSWVersion GetVersion()
        {
            IERSWVersion iERSW = new();
            object[] ret = _iERXmlRpcInterface.GetVersion();
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

        public static object[] GetCounter()
        {
            return _iERXmlRpcInterface.GetCounter();
        }

        public static object[] SetMode(string[] param)
        {
            return _iERXmlRpcInterface.SetMode(param);
        }
        public static object[] SetCredentials(string[] param)
        {
            return _iERXmlRpcInterface.SetCredentials(param);
        }
        public static object[] GetSetTempo(int[] param)
        {
            return _iERXmlRpcInterface.GetSetTempo(param);
        }
        public static object[] GetSetTempoFlow(int[] param)
        {
            return _iERXmlRpcInterface.GetSetTempoFlow(param);
        }
        public static object[] GetCurrentPassage()
        {
            return _iERXmlRpcInterface.GetCurrentPassage();
        }
        public static string methodHelp(string MethodName)
        {
            return _iERXmlRpcInterface.methodHelp(MethodName);
        }
        public static object[] SetOutputClient(int[] param)
        {
            return _iERXmlRpcInterface.SetOutputClient(param);
        }
        public static object[] GetMotorSpeed()
        {
            return _iERXmlRpcInterface.GetMotorSpeed();
        }
        public static object[] SetMotorSpeed(object[] param)
        {
            return _iERXmlRpcInterface.SetMotorSpeed(param);
        }

        public static object[] SetBuzzerFraud(int[] param)
        {
            return _iERXmlRpcInterface.SetBuzzerFraud(param);
        }

        public static object[] SetBuzzerIntrusion(int[] param)
        {
            return _iERXmlRpcInterface.SetBuzzerIntrusion(param);
        }

        public static object[] SetBuzzerMode(int[] param)
        {
            return _iERXmlRpcInterface.SetBuzzerMode(param);
        }
    }
}
