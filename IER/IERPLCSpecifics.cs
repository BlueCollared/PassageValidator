namespace IFS2.Equipment.HardwareInterface.IERPLCManager
{

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
        LANG_PREALARM_B = 0x1000,
        LANG_FRAUD_MANTRAP = 0x2000
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

    public enum ePassageTimeouts
    {
        LANG_ENTRY_TIMEOUT_A=0, //A passenger coming from the entrance (A side) did not cross the gate in the allotted time
        LANG_ENTRY_TIMEOUT_B=1, //A passenger coming from the entrance (B side) did not cross the gate in the allotted time
        LANG_EXIT_TIMEOUT=2, // The exit has not been cleared completely in the allotted time
        LANG_NO_CROSSING_TIMEOUT=4, //A passenger coming did not cross the gate in the allotted time
        LANG_NO_ENTRY_TIMEOUT=8, // Timeouts during boarding (the person did not enter the gate in the allotted time)
        LANG_SECURITY_TIMEOUT=16, //A passenger took too much time to exit the safety zone and prevents the closure of the doorsa
        LANG_VALIDATION_TIMEOUT=32        
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
        ERRORS_THERMOPILE, //Thermal sensor error
        ERRORS_CAMERA_VERSION,
        ERRORS_CUSTOMER,
        ERRORS_CUSTOMER_FAILURE,
        ERRORS_EXT,
        ERRORS_FRONTAL_DETECTION,
        ERRORS_FRONTAL_OCCULTED,
        ERRORS_HALL_EFFECT_FAILURE,
        ERRORS_LATERAL_DETECTION,
        ERRORS_LCD,
        ERRORS_MODBUS_SERIAL_FAILURE,
        ERRORS_MODBUS_TCP_FAILURE,
        ERRORS_MOTOR_BRAKE_FAILURE,
        ERRORS_OPENED_SERVICE_DOOR,
        ERRORS_READER,
        ERRORS_SOUNDPLAYER,
        ERRORS_SYSTEMD_FAILURE,
        ERRORS_USINE_TEST_WARNING,
    }   
}
