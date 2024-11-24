using Domain.Services.Modes;
using EtGate.Domain.Peripherals.Qr;

namespace GateApp
{
    public record ModuleAStatus;
    public record ModuleBStatus;
    // Application Layer
    public interface IStatusService
    {
        IObservable<EquipmentStatus> GetStatusUpdates();
    }

    public class StatusService : IStatusService
    {
        private readonly IObservable<QrReaderStatus> _moduleAStatusStream;
        private readonly IObservable<ModuleBStatus> _moduleBStatusStream;
        public StatusService(
        IObservable<QrReaderStatus> moduleAStatusStream,
            IObservable<ModuleBStatus> moduleBStatusStream)
        {
            _moduleAStatusStream = moduleAStatusStream;
            _moduleBStatusStream = moduleBStatusStream;
        }

        public IObservable<EquipmentStatus> GetStatusUpdates()
        {
            throw new NotImplementedException();
//            return _moduleAStatusStream
//                .Merge<EquipmentStatus>(_moduleBStatusStream);
        }
    }

}
