using System.Reactive.Subjects;

namespace Domain.Services.Modes
{
    public class ModeManager
    {
        private readonly ReplaySubject<EquipmentStatus> eqptStatusSubject;

        public ModeManager()
        {
            eqptStatusSubject = new ReplaySubject<EquipmentStatus>(bufferSize: 1);
        }
    }
}