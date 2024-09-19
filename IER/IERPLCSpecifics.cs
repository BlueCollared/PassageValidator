namespace IFS2.Equipment.HardwareInterface.IERPLCManager
{
    public enum eIERDoorsState
    {
        DOOR_CLOSED,
        DOOR_MOVING,
        DOOR_OPEN_A,
        DOOR_OPEN_B
    }

    public enum eIERPLCModes // state_machine modes    
    {
        EMERGENCY,
        LOCKED_OPEN, //forced open
        MAINTENANCE,
        NOMINAL,
        POWER_DOWN,
        TECHNICAL_FAILURE
    }


    

    public class cPLCStateMachine
    {
        //public eDoorsStatesMachine doorsStates;
        //public eDoorNominalModes doorNominalModes;
        public cPLCStateMachine()
        {
            //doorsStates = eDoorsStatesMachine.NONE;
          //  doorNominalModes = eDoorNominalModes.NONE;

        }
    }

    

    
}
