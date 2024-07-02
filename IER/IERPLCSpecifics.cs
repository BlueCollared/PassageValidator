namespace IFS2.Equipment.HardwareInterface.IERPLCManager
{
    public class IERSWVersion
    {
        public string LaneType;
        public string SWVersion;
        public string CompilationDate;
        public string GITVersion;
        public string GITDate;

        public IERSWVersion()
        {
            LaneType="";
            SWVersion= "";
            CompilationDate = "";
            GITVersion = "";
            GITDate = "";
        }
    }

    public enum eIERDoorModes
    {
        OP_MODE_BLOCK_CLOSED,
        OP_MODE_NC, //Normally Closed
        OP_MODE_NOA, //Normally Open in Entry
        OP_MODE_NOB, //Normally Open in Exit
        OP_MODE_OPTICAL_A, 
        OP_MODE_OPTICAL_B
    }

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

    public enum eIERPLCState
    {
        CLOSE_DOOR, //The passenger has crossed the obstacles or a timeout has occured : closing the doors
        FRAUD,// A fraud has occured
        INTRUSION,//An intrusion has occured
        OPEN_DOOR, //Boarding started : the door is open
        WAIT_FOR_AUTHORIZATION //The gate is ready to accept a new authorisation to start a boarding (or for a person to enter the gate)
    }

    public enum eInfractions
    {
        LANG_FRAUD_A = 0x01,
        LANG_FRAUD_B = 0x02,
        LANG_FRAUD_DISAPPEARANCE = 0x04,
        LANG_FRAUD_HOLDING = 0x08,
        LANG_FRAUD_JUMP = 0x10,
        LANG_FRAUD_RAMPING = 0x20,
        LANG_FRAUD_UNEXPECTED_MOTION = 0x40,
        LANG_INTRUSION_A= 0x80,
        LANG_INTRUSION_B= 0x100,
        LANG_OPPOSITE_INTRUSION_A = 0x200,
        LANG_OPPOSITE_INTRUSION_B = 0x400,
        LANG_PREALARM_A = 0x800,
        LANG_PREALARM_B = 0x1000
    }

    public class cPLCStateMachine
    {
        public eDoorsStatesMachine doorsStates;
        public eDoorNominalModes doorNominalModes;
        public cPLCStateMachine()
        {
            doorsStates = eDoorsStatesMachine.NONE;
            doorNominalModes = eDoorNominalModes.NONE;

        }
    }
    public enum eDoorsStatesMachine
    {
        NONE = -1,
        EGRESS,
        NOMINAL, //Normal mode .. if set then check for doors Nominal Mode 
        EMERGENCY,
        MAINTENANCE,
        LOCKED_OPEN,// FORCED_OPEN,
        POWER_DOWN,
        TECHNICAL_FAILURE //OUTOFORER
    }
    public enum eDoorNominalModes //Normal case
    {
        NONE = -1,
        OPEN_DOOR, //Doors are opened
        CLOSE_DOOR, //Doors are closed
        FRAUD, //A Fraud has occured
        INTRUSION, // Intrusion
        WAIT_FOR_AUTHORIZATION //The gate is ready to accept a new authorisation to start a boarding (or for a person to enter the gate)
    }

    public enum ePassageTimeouts
    {
        LANG_ENTRY_TIMEOUT_A=0, //A passenger coming from the entrance (A side) did not cross the gate in the allotted time
        LANG_ENTRY_TIMEOUT_B=1, //A passenger coming from the entrance (B side) did not cross the gate in the allotted time
        LANG_EXIT_TIMEOUT=2, // The exit has not been cleared completely in the allotted time
        LANG_NO_CROSSING_TIMEOUT=3, //A passenger coming did not cross the gate in the allotted time
        LANG_NO_ENTRY_TIMEOUT=4, // Timeouts during boarding (the person did not enter the gate in the allotted time)
        LANG_SECURITY_TIMEOUT=5, //A passenger took too much time to exit the safety zone and prevents the closure of the doorsa
        LANG_VALIDATION_TIMEOUT=6,
        LANG_NOENTRY_TIMEOUT=4
    }

    public enum eIERPLCErrors
    {
        ERRORS_BLOCKED_MOTOR,
        ERRORS_CAMERA_HEIGHT,
        ERRORS_CAMERA_NG,
        ERRORS_CAN,
        ERRORS_CAN_HEARTBEAT,
        ERRORS_CAN_OVERFLOW,
        ERRORS_CAN_PRODUCT_CODE,
        ERRORS_CAN_SW_VERSION,
        ERRORS_CELL_IR,
        ERRORS_CPU,
        ERRORS_DETECTION, // Sensors error
        ERRORS_DIRAS_EMITTER,// DIRS emmiters failure
        ERRORS_DIRAS_RECEIVER,
        ERRORS_EGRESS, //
        ERRORS_FTP,
        ERRORS_INSTALLATION,
        //Flaps error
        ERRORS_MOTOR_BRAKE,
        ERRORS_MOTOR_CARD,
        ERRORS_MOTOR_CARD_CONFIG,
        ERRORS_MOTOR_INIT,
        ERRORS_MOTOR_PEAK_CURRENT,
        ERRORS_OBSTRUCTED_CELLS,//One or more photocells are obstructed
        ERRORS_TEMPERATURE, // motor temp
        ERRORS_THERMOPILE //Thermal sensor error
    }   
}
