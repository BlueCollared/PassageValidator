using IFS2.Equipment.DriverInterface;
using LanguageExt;

namespace EtGate.IER
{
    public class Ier_To_DomainAdapter
    {
        IIERXmlRpc worker;

        public Ier_To_DomainAdapter(IIERXmlRpc worker)
        {
            this.worker = worker;
        }
        
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

        public bool SetOOO()
        {
            return worker.SetMode(DoorsMode.BlockClosed, Option<SideOperatingModes>.None, Option<SideOperatingModes>.None).IsRight;
        }

        public bool SetMode(Option<SideOperatingModes> entry, Option<SideOperatingModes> exit)
        {
            if (entry.IsNone && exit.IsNone)
                return true;

            return worker.SetMode(Option<DoorsMode>.None, entry, exit).IsRight;
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