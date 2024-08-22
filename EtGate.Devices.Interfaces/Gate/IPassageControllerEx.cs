using EtGate.Domain.Services.Gate;

namespace EtGate.Devices.Interfaces.Gate
{
    public interface IPassageControllerEx : IPassageController
    {
        // the application is supposed to save the Demanded state. It may be possible that the device is not reachable now, but as soon as it is reachable, the command would be fulfilled.

        void Reboot();
    }
}