using System.Collections;
using System.Xml.Serialization;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager;

public partial class CIERRpcHelper
{
    [XmlRoot("Data")]
    public class GetStatusStdAnswer
    {
        [XmlElement("AuthorizationA")]
        public int AuthorizationA = 0;
        [XmlElement("AuthorizationB")]
        public int AuthorizationB = 0;
        [XmlElement("PassageA")]
        public int PassageA = 0;
        [XmlElement("PassageB")]
        public int PassageB = 0;
        [XmlElement("Alarm")]
        public List<string> alarms = new List<string>();
        [XmlElement("Door_Mode")]
        public string door_mode = "";
        [XmlElement("Door")]
        public List<DictionaryXmlInt> doors = new List<DictionaryXmlInt>();
        [XmlElement("Door2")]
        public List<DictionaryXmlString> doors2 = new List<DictionaryXmlString>();
        [XmlElement("Failure")]
        public List<ForFailure> failures = new List<ForFailure>();
        [XmlElement("Inputs")]
        public List<DictionaryXmlInt> inputs = new List<DictionaryXmlInt>();
        [XmlElement("Operating_mode")]
        public List<DictionaryXmlString> operating_mode = new List<DictionaryXmlString>();
        [XmlElement("People")]
        public List<DictionaryXmlInt> people = new List<DictionaryXmlInt>();
        [XmlElement("Presence")]
        public List<DictionaryXmlInt> presence = new List<DictionaryXmlInt>();
        [XmlElement("Reader_lock")]
        public List<DictionaryXmlInt> reader_lock = new List<DictionaryXmlInt>();
        [XmlElement("State_Machine")]
        public List<DictionaryXmlString> state_machine = new List<DictionaryXmlString>();
        [XmlElement("Timeout")]
        public List<string> timeouts = new List<string>();        

        public GetStatusStdAnswer(object obj)
        {
            try
            {
                IDictionary idict = (IDictionary)obj;

                if (idict.Contains("AuthorizationA")) AuthorizationA = (int)(idict["AuthorizationA"]);
                if (idict.Contains("AuthorizationB")) AuthorizationB = (int)(idict["AuthorizationB"]);
                if (idict.Contains("PassageA")) AuthorizationA = (int)(idict["PassageA"]);
                if (idict.Contains("PassageB")) AuthorizationB = (int)(idict["PassageB"]);
                if (idict.Contains("people"))
                {
                    IDictionary idict1 = (IDictionary)(idict["people"]);
                    if (idict1.Contains("A")) people.Add(new DictionaryXmlInt("A", (int)(idict1["A"])));
                    if (idict1.Contains("B")) people.Add(new DictionaryXmlInt("B", (int)(idict1["B"])));
                }
                if (idict.Contains("presence"))
                {
                    IDictionary idict1 = (IDictionary)(idict["presence"]);
                    if (idict1.Contains("A")) presence.Add(new DictionaryXmlInt("A", (int)(idict1["A"])));
                    if (idict1.Contains("B")) presence.Add(new DictionaryXmlInt("B", (int)(idict1["B"])));
                }
                if (idict.Contains("reader_lock"))
                {
                    IDictionary idict1 = (IDictionary)(idict["reader_lock"]);
                    if (idict1.Contains("A")) reader_lock.Add(new DictionaryXmlInt("A", (int)(idict1["A"])));
                    if (idict1.Contains("B")) reader_lock.Add(new DictionaryXmlInt("B", (int)(idict1["B"])));
                }
                if (idict.Contains("door_mode")) door_mode = (string)(idict["door_mode"]);
                if (idict.Contains("alarms"))
                {
                    IList idict1 = (IList)(idict["alarms"]);
                    foreach (object o in idict1) alarms.Add((string)(o));
                }
                
                if (idict.Contains("doors"))
                {
                    IDictionary idict1 = (IDictionary)(idict["doors"]);
                    if (idict1.Contains("brake")) doors.Add(new DictionaryXmlInt("brake", (int)(idict1["brake"])));
                    if (idict1.Contains("error")) doors.Add(new DictionaryXmlInt("error", (int)(idict1["error"])));
                    if (idict1.Contains("initialized")) doors.Add(new DictionaryXmlInt("initialized", (int)(idict1["initialized"])));
                    if (idict1.Contains("safety_zone")) doors.Add(new DictionaryXmlInt("safety_zone", (int)(idict1["safety_zone"])));
                    if (idict1.Contains("safety_zone_A")) doors.Add(new DictionaryXmlInt("safety_zone_A", (int)(idict1["safety_zone_A"])));
                    if (idict1.Contains("safety_zone_B")) doors.Add(new DictionaryXmlInt("safety_zone_B", (int)(idict1["safety_zone_B"])));
                    if (idict1.Contains("state")) doors2.Add(new DictionaryXmlString("state", (string)(idict1["state"])));
                    //Logging.Verbose(idict1["state"].ToString());
                    if (idict1.Contains("unexpected_motion")) doors.Add(new DictionaryXmlInt("unexpected_motion", (int)(idict1["unexpected_motion"])));
                }
                
                if (idict.Contains("inputs"))
                {
                    IDictionary idict1 = (IDictionary)(idict["inputs"]);
                    if (idict1.Contains("customer_1")) inputs.Add(new DictionaryXmlInt("customer_1", (int)(idict1["customer_1"])));
                    if (idict1.Contains("customer_2")) inputs.Add(new DictionaryXmlInt("customer_2", (int)(idict1["customer_2"])));
                    if (idict1.Contains("customer_3")) inputs.Add(new DictionaryXmlInt("customer_3", (int)(idict1["customer_3"])));
                    if (idict1.Contains("customer_4")) inputs.Add(new DictionaryXmlInt("customer_4", (int)(idict1["customer_4"])));
                    if (idict1.Contains("customer_5")) inputs.Add(new DictionaryXmlInt("customer_5", (int)(idict1["customer_5"])));
                    if (idict1.Contains("customer_6")) inputs.Add(new DictionaryXmlInt("customer_6", (int)(idict1["customer_6"])));
                    if (idict1.Contains("emergency")) inputs.Add(new DictionaryXmlInt("emergency", (int)(idict1["emergency"])));
                    if (idict1.Contains("entry_locked_open")) inputs.Add(new DictionaryXmlInt("entry_locked_open", (int)(idict1["entry_locked_open"])));
                    if (idict1.Contains("ups")) inputs.Add(new DictionaryXmlInt("ups", (int)(idict1["ups"])));
                }
                if (idict.Contains("state_machine"))
                {
                    IDictionary idict1 = (IDictionary)(idict["state_machine"]);
                    if (idict1.Contains("mode")) state_machine.Add(new DictionaryXmlString("mode", (string)(idict1["mode"])));
                    if (idict1.Contains("state")) state_machine.Add(new DictionaryXmlString("state", (string)(idict1["state"])));
                }
                if (idict.Contains("operating_mode"))
                {
                    IDictionary idict1 = (IDictionary)(idict["operating_mode"]);
                    if (idict1.Contains("A")) operating_mode.Add(new DictionaryXmlString("A", (string)(idict1["A"])));
                    if (idict1.Contains("B")) operating_mode.Add(new DictionaryXmlString("B", (string)(idict1["B"])));
                }
                if (idict.Contains("failures"))
                {
                    IDictionary idict1 = (IDictionary)(idict["failures"]);
                    if (idict1.Contains("major"))
                    {
                        IList idict2 = (IList)(idict1["major"]);
                        List<string> l = new List<string>();
                        foreach (object o in idict2) l.Add((string)(o));
                        failures.Add(new ForFailure("major", l));
                    }
                    if (idict1.Contains("minor"))
                    {
                        IList idict2 = (IList)(idict1["minor"]);
                        List<string> l = new List<string>();
                        foreach (object o in idict2) l.Add((string)(o));
                        if (l.Count > 0) failures.Add(new ForFailure("minor", l));
                    }
                }
                if (idict.Contains("timeouts"))
                {
                    IList idict1 = (IList)(idict["timeouts"]);
                    foreach (object o in idict1) timeouts.Add((string)(o));
                }                
            }
            catch (Exception e)
            {
                throw e;
            }            
        }
    }
}
