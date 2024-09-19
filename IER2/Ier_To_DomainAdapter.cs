using LanguageExt;
using SideOperatingMode = EtGate.IER.SideOperatingMode;

namespace EtGate.IER
{
    public class Ier_To_DomainAdapter
    {
        IIerXmlRpc worker;

        public Ier_To_DomainAdapter(IIerXmlRpc worker)
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
            return worker.SetMode(DoorsMode.LockClosed, Option<SideOperatingMode>.None, Option<SideOperatingMode>.None).IsRight;
        }

        public bool SetMaintenance()
        {
            return worker.SetMaintenanceMode(true).IsRight;
        }

        public bool SetEmergency()
        {
            return worker.SetEmergency(true).IsRight;
        }

        public bool SetNormalMode(Option<SideOperatingMode> entry, Option<SideOperatingMode> exit)
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