using Domain.Peripherals.Passage;
using IFS2.Equipment.DriverInterface;
using IFS2.Equipment.HardwareInterface.IERPLCManager;
using LanguageExt;

namespace EtGate.IER
{
    internal struct GetStatusStdRaw
    {
        public Option<int> AuthorizationA;
        public Option<int> AuthorizationB;
        public Option<int> PassageA;
        public Option<int> PassageB;
        public Option<DoorsMode> door_mode;
        public Option<string> operatingModeSideA;
        public Option<string> operatingModeSideB;
        public Option<int> peopleDetectedInsideA;
        public Option<int> peopleDetectedInsideB;
        public Option<int> presencesDetectedInsideA;
        public Option<int> presencesDetectedInsideB;
        public Option<int> readerLockedSideA;
        public Option<int> readerLockedSideB;
        public Option<eDoorsStatesMachine> exceptionMode;
        public Option<eDoorNominalModes> stateIfInNominal;
        public Option<int> customer_1;
        public Option<int> customer_2;
        public Option<int> customer_3;
        public Option<int> customer_4;
        public Option<int> customer_5;
        public Option<int> customer_6;
        public Option<int> emergency;
        public Option<int> entryLockOpen;
        public Option<int> ups;
        public List<eIERPLCErrors> majorTechnicalFailures;
        public List<eIERPLCErrors> minorTechnicalFailures;

        public Option<int> timeouts;
        public Option<int> alarms;

        public Option<bool> brakeEnabled;
        public Option<bool> doorFailure;
        public Option<bool> doorinitialized;
        public Option<bool> safety_zone;
        public Option<bool> safety_zone_A;
        public Option<bool> safety_zone_B;
        public Option<int> doorCurrentMovementOfObstacle;
        public Option<bool> door_unexpected_motion;

    }
}
