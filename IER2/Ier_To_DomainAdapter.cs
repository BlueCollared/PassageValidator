using LanguageExt;

namespace EtGate.IER
{
    public class Ier_To_DomainAdapter
    {
        IIERXmlRpc worker;
        public bool Reboot()
        {
            return worker.Reboot().IsRight;
        }

        public bool Restart()
        {
            return worker.Restart().IsRight;
        }

        public bool SetDate(DateTime dt)
        {
            return worker.SetDate(dt).IsRight;
        }

        public Option<DateTime> GetDate()
        {
            return worker.GetDate().ToOption();
        }

        // TODO: IERStatus is not suitable for domain consumption. The function should return an adapted version of IERStatus
        public Option<IERStatus> GetStatus()
        {
            return worker.GetStatus().ToOption();
        }
    }
}