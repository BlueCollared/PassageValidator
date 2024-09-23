using EtGate.Domain.Services.Gate;
using LanguageExt;

namespace EtGate.IER;

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

    public bool SetOOS()
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

    public bool SetNormalMode(GateOperationConfig cfg)
    {        
        Option<SideOperatingMode> entry = cfg.mode switch
        {
            Mode.EntryOnly_EntryFree => SideOperatingMode.Free,
            Mode.EntryOnly_EntryControlled => SideOperatingMode.Controlled,
            Mode.ExitOnly_ExitFree => SideOperatingMode.Closed,
            Mode.ExitOnly_ExitControlled => SideOperatingMode.Closed,
            Mode.BiDi_EntryFree_ExitFree => SideOperatingMode.Free,
            Mode.BiDi_EntryFree_ExitControlled => SideOperatingMode.Free,
            Mode.BiDi_EntryControlled_ExitFree => SideOperatingMode.Controlled,
            Mode.BiDi_EntryControlled_ExitControlled => SideOperatingMode.Controlled,
            _ => default
        };

        Option<SideOperatingMode> exit = cfg.mode switch
        {
            Mode.EntryOnly_EntryFree => SideOperatingMode.Closed,
            Mode.EntryOnly_EntryControlled => SideOperatingMode.Closed,
            Mode.ExitOnly_ExitFree => SideOperatingMode.Free,
            Mode.ExitOnly_ExitControlled => SideOperatingMode.Controlled,
            Mode.BiDi_EntryFree_ExitFree => SideOperatingMode.Free,
            Mode.BiDi_EntryFree_ExitControlled => SideOperatingMode.Controlled,
            Mode.BiDi_EntryControlled_ExitFree => SideOperatingMode.Free,
            Mode.BiDi_EntryControlled_ExitControlled => SideOperatingMode.Controlled,
            _ => default
        };

        if (entry.IsNone || exit.IsNone)
            return false;

        if (cfg.bNormallyClosed)
            return worker.SetMode(DoorsMode.NormallyClosed, entry, exit).IsRight;
        else
        {
            Option<DoorsMode> dm =
            cfg.mode switch
            {
                Mode.EntryOnly_EntryFree => DoorsMode.NormallyOpenedA,
                Mode.EntryOnly_EntryControlled => DoorsMode.NormallyOpenedA,
                Mode.ExitOnly_ExitFree => DoorsMode.NormallyOpenedB,
                Mode.ExitOnly_ExitControlled => DoorsMode.NormallyOpenedB,
                Mode.BiDi_EntryFree_ExitFree => DoorsMode.OpticalA,
                Mode.BiDi_EntryFree_ExitControlled => DoorsMode.NormallyOpenedB,
                Mode.BiDi_EntryControlled_ExitFree => DoorsMode.NormallyOpenedA,
                Mode.BiDi_EntryControlled_ExitControlled => DoorsMode.NormallyOpenedA,
                _ => default
            };
            if (dm.IsNone)
                return false;

            return worker.SetMode(dm, entry, exit).IsRight;
        }
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