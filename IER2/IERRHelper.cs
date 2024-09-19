using Domain.Peripherals.Passage;
using EtGate.IER;
using IFS2.Common;
using IFS2.Equipment.DriverInterface;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;
using LanguageExt.Pipes;
using LanguageExt.TypeClasses;
using LanguageExt.UnitsOfMeasure;
using OneOf.Types;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static IFS2.Equipment.HardwareInterface.IERPLCManager.CIERRpcHelper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GlobalEquipmentDirection = IFS2.Equipment.DriverInterface.GlobalEquipmentDirection;
using GlobalEquipmentEntryMode = IFS2.Equipment.DriverInterface.GlobalEquipmentEntryMode;
using GlobalEquipmentExitMode = IFS2.Equipment.DriverInterface.GlobalEquipmentExitMode;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager
{
    public record DoorModeConf
        (
        bool ForceDoorinReverseSide,
        bool ForceDoorinEntrySide,
        bool ForceDoorinFreeSide
        );
    public enum eDoorNominalModes //Normal case
    {
        NONE = -1,
        OPEN_DOOR, //Doors are opened
        CLOSE_DOOR, //Doors are closed
        FRAUD, //A Fraud has occured
        INTRUSION, // Intrusion
        WAIT_FOR_AUTHORIZATION //The gate is ready to accept a new authorisation to start a boarding (or for a person to enter the gate)
    }



    internal static class IERRHelper
    {
        public enum GetStatusResponseRawError
        {
            InvalidLength,
            F0,
            F1,
            F2,
            F3,
            F4,
            F5,
            F6,
            F7,
            F8,
            F9,
            F10,
            F11,
            F12,
            F13,
            F14,
            F15,
        }

        static readonly Dictionary<string, DoorsMode> doorsModeDi = new(){
            { "00318", DoorsMode.LockClosed },
            { "00320", DoorsMode.OpticalA },
            { "00321", DoorsMode.OpticalB },
            { "00323", DoorsMode.NormallyClosed },
            { "00325", DoorsMode.NormallyOpenedA },
            { "00326", DoorsMode.NormallyOpenedB },
            { "00327", DoorsMode.LockedOpenA },
            { "00328", DoorsMode.LockedOpenB }
        };

        static readonly Dictionary<string, DoorsMode> doorsModeDi2 = new(){
            { "OP_MODE_BLOCK_CLOSED'", DoorsMode.LockClosed },
            { "OP_MODE_OPTICAL_A", DoorsMode.OpticalA },
            { "OP_MODE_OPTICAL_B", DoorsMode.OpticalB },
            { "OP_MODE_NC", DoorsMode.NormallyClosed },
            { "OP_MODE_NOA", DoorsMode.NormallyOpenedA },
            { "OP_MODE_NOB", DoorsMode.NormallyOpenedB },
            { "OP_MODE_LOCKED_OPEN_A'", DoorsMode.LockedOpenA },
            { "OP_MODE_LOCKED_OPEN_B", DoorsMode.LockedOpenB }
        };

        static readonly Dictionary<string, SideOperatingModes> sideOpModeDi = new() {
            {"00315", SideOperatingModes.Controlled },
            {"00316", SideOperatingModes.Free },
            {"00317", SideOperatingModes.Closed },
        };

        static readonly Dictionary<string, eInfractions> infractionsDi = new(){
            { "01005", eInfractions.LANG_INTRUSION_A },
            { "01006", eInfractions.LANG_INTRUSION_B },
            { "01007", eInfractions.LANG_OPPOSITE_INTRUSION_A },
            { "01008", eInfractions.LANG_OPPOSITE_INTRUSION_B },
            { "01012", eInfractions.LANG_FRAUD_A },
            { "01013", eInfractions.LANG_FRAUD_B },
            { "01047", eInfractions.LANG_FRAUD_DISAPPEARANCE },
            { "01048", eInfractions.LANG_FRAUD_RAMPING },
            { "01052", eInfractions.LANG_FRAUD_JUMP },
            { "01055", eInfractions.LANG_FRAUD_HOLDING },
            { "01056", eInfractions.LANG_PREALARM_A },
            { "01057", eInfractions.LANG_PREALARM_B },
            { "01063", eInfractions.LANG_FRAUD_MANTRAP },
            { "01066", eInfractions.LANG_FRAUD_UNEXPECTED_MOTION }
        };

        static readonly Dictionary<string, eInfractions> infractionsDi2 = new()
        {
            { "LANG_FRAUD_A", eInfractions.LANG_FRAUD_A },
            { "LANG_FRAUD_B", eInfractions.LANG_FRAUD_B },
            { "LANG_FRAUD_DISAPPEARANCE", eInfractions.LANG_FRAUD_DISAPPEARANCE },
            { "LANG_FRAUD_HOLDING", eInfractions.LANG_FRAUD_HOLDING },
            { "LANG_FRAUD_JUMP", eInfractions.LANG_FRAUD_JUMP },
            { "LANG_FRAUD_RAMPING", eInfractions.LANG_FRAUD_RAMPING },
            { "LANG_FRAUD_UNEXPECTED_MOTION", eInfractions.LANG_FRAUD_UNEXPECTED_MOTION },
            { "LANG_INTRUSION_A", eInfractions.LANG_INTRUSION_A },
            { "LANG_INTRUSION_B", eInfractions.LANG_INTRUSION_B },
            { "LANG_OPPOSITE_INTRUSION_A", eInfractions.LANG_OPPOSITE_INTRUSION_A },
            { "LANG_OPPOSITE_INTRUSION_B", eInfractions.LANG_OPPOSITE_INTRUSION_B },
            { "LANG_PREALARM_A", eInfractions.LANG_PREALARM_A },
            { "LANG_PREALARM_B", eInfractions.LANG_PREALARM_B },
            { "LANG_FRAUD_MANTRAP", eInfractions.LANG_FRAUD_MANTRAP }
        };

        static readonly Dictionary<string, ePassageTimeouts> timeoutsDi = new()
        {
            { "01009", ePassageTimeouts.LANG_ENTRY_TIMEOUT_A },
            { "01010", ePassageTimeouts.LANG_ENTRY_TIMEOUT_B },
            { "01011", ePassageTimeouts.LANG_EXIT_TIMEOUT },
            { "01045", ePassageTimeouts.LANG_NO_ENTRY_TIMEOUT },
            { "01046", ePassageTimeouts.LANG_NO_CROSSING_TIMEOUT },
            { "01054", ePassageTimeouts.LANG_SECURITY_TIMEOUT },
            { "01059", ePassageTimeouts.LANG_VALIDATION_TIMEOUT }
        };

        static readonly Dictionary<string, ePassageTimeouts> timeoutsDi2 = new()
        {
            { "LANG_ENTRY_TIMEOUT_A", ePassageTimeouts.LANG_ENTRY_TIMEOUT_A },
            { "LANG_ENTRY_TIMEOUT_B", ePassageTimeouts.LANG_ENTRY_TIMEOUT_B },
            { "LANG_EXIT_TIMEOUT", ePassageTimeouts.LANG_EXIT_TIMEOUT },
            { "LANG_NO_ENTRY_TIMEOUT", ePassageTimeouts.LANG_NO_ENTRY_TIMEOUT },
            { "LANG_NO_CROSSING_TIMEOUT", ePassageTimeouts.LANG_NO_CROSSING_TIMEOUT },
            { "LANG_SECURITY_TIMEOUT", ePassageTimeouts.LANG_SECURITY_TIMEOUT },
            { "LANG_VALIDATION_TIMEOUT", ePassageTimeouts.LANG_VALIDATION_TIMEOUT }
        };

        static readonly Dictionary<string, eDoorsStatesMachine> exceptionModeDi = new()
        {
            {"EGRESS", eDoorsStatesMachine.EGRESS },
            {"EMERGENCY", eDoorsStatesMachine.EMERGENCY },
            {"LOCKED_OPEN", eDoorsStatesMachine.LOCKED_OPEN },
            {"MAINTENANCE", eDoorsStatesMachine.MAINTENANCE },
            {"NOMINAL", eDoorsStatesMachine.NOMINAL },
            {"POWER_DOWN", eDoorsStatesMachine.POWER_DOWN },
            {"TECHNICAL_FAILURE", eDoorsStatesMachine.TECHNICAL_FAILURE }
        };

        static readonly Dictionary<string, eDoorNominalModes> doorsModeDi3 = new()
        {
            { "OPEN_DOOR", eDoorNominalModes.OPEN_DOOR },
            { "CLOSE_DOOR", eDoorNominalModes.CLOSE_DOOR },
            { "FRAUD", eDoorNominalModes.FRAUD },
            { "INTRUSION", eDoorNominalModes.INTRUSION },
            { "WAIT_FOR_AUTHORIZATION", eDoorNominalModes.WAIT_FOR_AUTHORIZATION }
        };

        static readonly Dictionary<string, eIERPLCErrors> plcErrors = new()
        {
            {"ERRORS_BLOCKED_MOTOR", eIERPLCErrors.ERRORS_BLOCKED_MOTOR },
            {"ERRORS_CAMERA_HEIGHT", eIERPLCErrors.ERRORS_CAMERA_HEIGHT },
            {"ERRORS_CAMERA_NG", eIERPLCErrors.ERRORS_CAMERA_NG },
            {"ERRORS_CAMERA_VERSION", eIERPLCErrors.ERRORS_CAMERA_VERSION },
            {"ERRORS_CAN", eIERPLCErrors.ERRORS_CAN },
            {"ERRORS_CAN_HEARTBEAT", eIERPLCErrors.ERRORS_CAN_HEARTBEAT },
            {"ERRORS_CAN_OVERFLOW", eIERPLCErrors.ERRORS_CAN_OVERFLOW },
            {"ERRORS_CAN_PRODUCT_CODE", eIERPLCErrors.ERRORS_CAN_PRODUCT_CODE },
            {"ERRORS_CAN_SW_VERSION", eIERPLCErrors.ERRORS_CAN_SW_VERSION },
            {"ERRORS_CELL_IR", eIERPLCErrors.ERRORS_CELL_IR },
            {"ERRORS_CPU", eIERPLCErrors.ERRORS_CPU },
            {"ERRORS_CUSTOMER", eIERPLCErrors.ERRORS_CUSTOMER },
            {"ERRORS_CUSTOMER_FAILURE", eIERPLCErrors.ERRORS_CUSTOMER_FAILURE },
            {"ERRORS_DETECTION", eIERPLCErrors.ERRORS_DETECTION },
            {"ERRORS_DIRAS_EMITTER", eIERPLCErrors.ERRORS_DIRAS_EMITTER },
            {"ERRORS_DIRAS_RECEIVER", eIERPLCErrors.ERRORS_DIRAS_RECEIVER },
            {"ERRORS_EGRESS", eIERPLCErrors.ERRORS_EGRESS },
            {"ERRORS_EXT", eIERPLCErrors.ERRORS_EXT },
            {"ERRORS_FRONTAL_DETECTION", eIERPLCErrors.ERRORS_FRONTAL_DETECTION },
            {"ERRORS_FRONTAL_OCCULTED", eIERPLCErrors.ERRORS_FRONTAL_OCCULTED },
            {"ERRORS_FTP", eIERPLCErrors.ERRORS_FTP },
            {"ERRORS_HALL_EFFECT_FAILURE", eIERPLCErrors.ERRORS_HALL_EFFECT_FAILURE },
            {"ERRORS_INSTALLATION", eIERPLCErrors.ERRORS_INSTALLATION },
            {"ERRORS_LATERAL_DETECTION", eIERPLCErrors.ERRORS_LATERAL_DETECTION },
            {"ERRORS_LCD", eIERPLCErrors.ERRORS_LCD },
            {"ERRORS_MODBUS_SERIAL_FAILURE", eIERPLCErrors.ERRORS_MODBUS_SERIAL_FAILURE },
            {"ERRORS_MODBUS_TCP_FAILURE", eIERPLCErrors.ERRORS_MODBUS_TCP_FAILURE },
            {"ERRORS_MOTOR_BRAKE", eIERPLCErrors.ERRORS_MOTOR_BRAKE },
            {"ERRORS_MOTOR_BRAKE_FAILURE", eIERPLCErrors.ERRORS_MOTOR_BRAKE_FAILURE },
            {"ERRORS_MOTOR_CARD", eIERPLCErrors.ERRORS_MOTOR_CARD },
            {"ERRORS_MOTOR_CARD_CONFIG", eIERPLCErrors.ERRORS_MOTOR_CARD_CONFIG },
            {"ERRORS_MOTOR_INIT", eIERPLCErrors.ERRORS_MOTOR_INIT },
            {"ERRORS_MOTOR_PEAK_CURRENT", eIERPLCErrors.ERRORS_MOTOR_PEAK_CURRENT },
            {"ERRORS_OBSTRUCTED_CELLS", eIERPLCErrors.ERRORS_OBSTRUCTED_CELLS },
            {"ERRORS_OPENED_SERVICE_DOOR", eIERPLCErrors.ERRORS_OPENED_SERVICE_DOOR },
            {"ERRORS_READER", eIERPLCErrors.ERRORS_READER },
            {"ERRORS_SOUNDPLAYER", eIERPLCErrors.ERRORS_SOUNDPLAYER },
            {"ERRORS_SYSTEMD_FAILURE", eIERPLCErrors.ERRORS_SYSTEMD_FAILURE },
            {"ERRORS_TEMPERATURE", eIERPLCErrors.ERRORS_TEMPERATURE },
            {"ERRORS_THERMOPILE", eIERPLCErrors.ERRORS_THERMOPILE },
            {"ERRORS_USINE_TEST_WARNING", eIERPLCErrors.ERRORS_USINE_TEST_WARNING }
        };

        static public Either<List<string>, GetStatusStdRaw> ProcessGetStatusStd(object ip)
        {
            if (ip == null)
                return (new List<string> { "null input" });
            if (!(ip is IDictionary<string, object> idict))
                return (new List<string> { "input not a dictionary" });

            ConcurrentBag<string> errors = new();
            ConcurrentBag<string> warnings = new();

            GetStatusStdRaw res = new();

            // TODO: see if AsParallel() is really needed, specially when gate is in nominal mode. In fact, it could be harming the performance
            idict.AsParallel().ForAll(kvp =>
            {
                switch (kvp.Key)
                {
                    case "AuthorizationA":
                        {
                            if (kvp.Value is int i)
                                res.AuthorizationA = i;
                            else
                                errors.Add("AuthorizationA");
                            break;
                        }
                    case "AuthorizationB":
                        {
                            if (kvp.Value is int i)
                                res.AuthorizationB = i;
                            else
                                errors.Add("AuthorizationB");
                            break;
                        }
                    case "PassageA":
                        {
                            if (kvp.Value is int i)
                                res.PassageA = i;
                            else
                                errors.Add("PassageA");
                            break;
                        }
                    case "PassageB":
                        {
                            if (kvp.Value is int i)
                                res.PassageB = i;
                            else
                                errors.Add("PassageB");
                            break;
                        }
                    case "alarms":
                        {
                            int alarms = 0;
                            if (kvp.Value is object[] a)
                            {
                                foreach (var o in a)
                                {
                                    if (o is string s)
                                    {
                                        if (infractionsDi2.TryGetValue(s, out eInfractions v))
                                            alarms |= (int)v;
                                    }
                                    else
                                        errors.Add("alarms value is not string");
                                }

                                res.infractions = alarms;
                            }
                            else
                                errors.Add("alarms");
                            break;
                        }
                    case "door_mode":
                        {
                            if (kvp.Value is string s)
                            {
                                if (doorsModeDi2.TryGetValue(s, out DoorsMode v))
                                    res.door_mode = v;
                                else
                                    errors.Add("door_mode value " + s);
                            }
                            else
                                errors.Add("door_mode");
                            break;
                        }
                    case "doors":
                        {
                            var di = kvp.Value as IDictionary<string, object>;
                            if (di == null)
                            {
                                errors.Add("doors is not list");
                                break;
                            }
                            foreach (var kvp1 in di)
                            {
                                switch (kvp1.Key)
                                {
                                    case "brake":
                                        {
                                            if (kvp1.Value is int i)
                                                res.brakeEnabled = i == 1;
                                            else
                                                errors.Add("brake");
                                            break;
                                        }
                                    case "error":
                                        {
                                            if (kvp1.Value is int i)
                                                res.doorFailure = i == 1;
                                            else
                                                errors.Add("error");
                                            break;
                                        }
                                    case "initialized":
                                        {
                                            if (kvp1.Value is int i)
                                                res.doorinitialized = i == 1;
                                            else
                                                errors.Add("initialized");
                                            break;
                                        }
                                    case "safety_zone":
                                        {
                                            if (kvp1.Value is int i)
                                                res.safety_zone = i == 1;
                                            else
                                                errors.Add("safety_zone");
                                            break;
                                        }
                                    case "safety_zone_A":
                                        {
                                            if (kvp1.Value is int i)
                                                res.safety_zone_A = i == 1;
                                            else
                                                errors.Add("safety_zone_A");
                                            break;
                                        }
                                    case "safety_zone_B":
                                        {
                                            if (kvp1.Value is int i)
                                                res.safety_zone_B = i == 1;
                                            else
                                                errors.Add("safety_zone_B");
                                            break;
                                        }
                                    case "state":
                                        {
                                            if (kvp1.Value is int i)
                                                res.doorCurrentMovementOfObstacle = i;
                                            else
                                                errors.Add("state");
                                            break;
                                        }
                                    case "unexpected_motion":
                                        {
                                            if (kvp1.Value is int i)
                                                res.door_unexpected_motion = i == 1;
                                            else
                                                errors.Add("unexpected_motion");
                                            break;
                                        }
                                }
                                break;
                            }
                            break;
                        }
                    case "failures":
                        {
                            var di = kvp.Value as IDictionary<string, object>;
                            if (di == null)
                            {
                                errors.Add("failures is not dictionary");
                                break;
                            }
                            {
                                di.TryGetValue("major", out object major);

                                if (!(major is IList a))
                                {
                                    errors.Add("failures.major");
                                    break;
                                }

                                res.majorTechnicalFailures = new();
                                foreach (object o in a)
                                {
                                    if (o is string s)
                                        if (plcErrors.TryGetValue(s, out eIERPLCErrors v))
                                            res.majorTechnicalFailures.Add(v);
                                        else
                                        {
                                            warnings.Add("failures.major " + s);
                                            //break;
                                        }
                                    else
                                    {
                                        errors.Add("unexpected failure isn't string");
                                        break;
                                    }
                                }
                            }

                            {
                                di.TryGetValue("minor", out object minor);

                                if (!(minor is IList a))
                                {
                                    errors.Add("failures.minor");
                                    break;
                                }

                                res.minorTechnicalFailures = new();
                                foreach (object o in a)
                                {
                                    if (o is string s)
                                        if (plcErrors.TryGetValue(s, out eIERPLCErrors v))
                                            res.minorTechnicalFailures.Add(v);
                                        else
                                        {
                                            warnings.Add("failures.minor " + s);
                                            //break;
                                        }
                                    else
                                    {
                                        errors.Add("unexpected failure isn't string");
                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    case "inputs":
                        {
                            var di = kvp.Value as IDictionary<string, object>;
                            if (di == null)
                            {
                                errors.Add("inputs is not dictionary");
                                break;
                            }

                            int customersActive = 0;
                            foreach (var kvp1 in di)
                            {
                                switch (kvp1.Key)
                                {
                                    case "customer_1":
                                        {
                                            if (kvp1.Value is int i)
                                                customersActive = i |= 1;
                                            else
                                                errors.Add("customer_1");
                                            break;
                                        }
                                    case "customer_2":
                                        {
                                            if (kvp1.Value is int i)
                                                customersActive = i |= 2;
                                            else
                                                errors.Add("customer_2");
                                            break;
                                        }
                                    case "customer_3":
                                        {
                                            if (kvp1.Value is int i)
                                                customersActive = i |= 4;
                                            else
                                                errors.Add("customer_3");
                                            break;
                                        }
                                    case "customer_4":
                                        {
                                            if (kvp1.Value is int i)
                                                customersActive = i |= 8;
                                            else
                                                errors.Add("customer_4");
                                            break;
                                        }
                                    case "customer_5":
                                        {
                                            if (kvp1.Value is int i)
                                                customersActive = i |= 16;
                                            else
                                                errors.Add("customer_5");
                                            break;
                                        }
                                    case "customer_6":
                                        {
                                            if (kvp1.Value is int i)
                                                customersActive = i |= 32;
                                            else
                                                errors.Add("customer_6");
                                            break;
                                        }
                                    case "emergency":
                                        {
                                            if (kvp1.Value is int i)
                                                res.emergencyButton = i == 1;
                                            else
                                                errors.Add("emergency");
                                            break;
                                        }
                                    case "entry_lock_open":
                                        {
                                            if (kvp1.Value is int i)
                                                res.entryLockOpen = i == 1;
                                            else
                                                errors.Add("entry_lock_open");
                                            break;
                                        }
                                    case "ups":
                                        {
                                            if (kvp1.Value is int i)
                                                res.systemPoweredByUPS = i == 1;
                                            else
                                                errors.Add("ups");
                                            break;
                                        }
                                }
                                res.customersActive = customersActive;
                            }
                            break;
                        }
                    case "people":
                        {
                            var di = kvp.Value as IDictionary<string, object>;
                            if (di == null)
                            {
                                errors.Add("people is not dictionary");
                                break;
                            }
                            foreach (var kvp1 in di)
                            {
                                switch (kvp1.Key)
                                {
                                    case "A":
                                        {
                                            if (kvp1.Value is int i)
                                                res.peopleDetectedInsideA = i;
                                            else
                                                errors.Add("people.A");
                                            break;
                                        }
                                    case "B":
                                        {
                                            if (kvp1.Value is int i)
                                                res.peopleDetectedInsideB = i;
                                            else
                                                errors.Add("people.B");
                                            break;
                                        }
                                    default:
                                        warnings.Add("people bad key" + kvp1.Key);
                                        break;
                                }
                            }
                            break;
                        }
                    case "presence":
                        {
                            var di = kvp.Value as IDictionary<string, object>;
                            if (di == null)
                            {
                                errors.Add("presence is not dictionary");
                                break;
                            }
                            foreach (var kvp1 in di)
                            {
                                switch (kvp1.Key)
                                {
                                    case "A":
                                        {
                                            if (kvp1.Value is int i)
                                                res.presencesDetectedInsideA = i;
                                            else
                                                errors.Add("presence.A");
                                            break;
                                        }
                                    case "B":
                                        {
                                            if (kvp1.Value is int i)
                                                res.presencesDetectedInsideB = i;
                                            else
                                                errors.Add("presence.B");
                                            break;
                                        }
                                    default:
                                        warnings.Add("presence bad key" + kvp1.Key);
                                        break;
                                }
                            }
                            break;
                        }
                    case "reader_lock":
                        {
                            var di = kvp.Value as IDictionary<string, object>;
                            if (di == null)
                            {
                                errors.Add("reader_lock is not dictionary");
                                break;
                            }
                            foreach (var kvp1 in di)
                            {
                                switch (kvp1.Key)
                                {
                                    case "A":
                                        {
                                            if (kvp1.Value is int i)
                                                res.readerLockedSideA = i;
                                            else
                                                errors.Add("reader_lock.A");
                                            break;
                                        }
                                    case "B":
                                        {
                                            if (kvp1.Value is int i)
                                                res.readerLockedSideB = i;
                                            else
                                                errors.Add("reader_lock.B");
                                            break;
                                        }
                                    default:
                                        warnings.Add("reader_lock bad key" + kvp1.Key);
                                        break;
                                }
                            }
                            break;
                        }
                    case "state_machine":
                        {
                            var di = kvp.Value as IDictionary<string, object>;
                            if (di == null)
                            {
                                errors.Add("state_machine is not dictionary");
                                break;
                            }
                            foreach (var kvp1 in di)
                            {
                                switch (kvp1.Key)
                                {
                                    case "exception_mode":
                                        {
                                            if (kvp1.Value is string s)
                                            {
                                                if (exceptionModeDi.TryGetValue(s, out eDoorsStatesMachine v))
                                                    res.exceptionMode = v;
                                                else
                                                    errors.Add("exception_mode value " + s);
                                            }
                                            else
                                                errors.Add("exception_mode");
                                            break;
                                        }
                                    case "state_if_in_nominal":
                                        {
                                            if (kvp1.Value is string s)
                                            {
                                                if (doorsModeDi3.TryGetValue(s, out eDoorNominalModes v))
                                                    res.stateIfInNominal = v;
                                                else
                                                    errors.Add("state_if_in_nominal value " + s);
                                            }
                                            else
                                                errors.Add("state_if_in_nominal");
                                            break;
                                        }
                                    default:
                                        warnings.Add("state_machine bad key" + kvp1.Key);
                                        break;
                                }
                            }
                            break;
                        }
                    case "timeouts":
                        {
                            var idict1 = kvp.Value as IList;
                            if (idict1 == null)
                            {
                                errors.Add("timeouts is not list");
                                break;
                            }

                            if (kvp.Value is object[] a)
                            {
                                int timeouts = 0;
                                foreach (var o in a)
                                {
                                    if (o is string s)
                                    {
                                        if (timeoutsDi2.TryGetValue(s, out ePassageTimeouts v))
                                            timeouts |= (int)v;
                                        else
                                            errors.Add("timeouts value " + s);
                                    }
                                    else
                                        errors.Add("timeouts value is not string");
                                }
                                res.timeouts = timeouts;
                            }
                            else
                                errors.Add("timeouts");
                            break;
                        }
                }
            });
            return errors.Count > 0 ? errors.ToList() : res;
        }

        static public GetStatusResponseLittleProcessed Process(GetStatusResponseRaw raw)
        {
            GetStatusResponseLittleProcessed res = new();
            res.doorMode = doorsModeDi[raw.doorOperationgMode];
            res.opModeEntry = sideOpModeDi[raw.operatingModeEntrySide];
            res.opModeExit = sideOpModeDi[raw.operatingModeExitSide];
            res.infractions = raw.infractions.Select(s => infractionsDi[s]).ToArr();
            res.timeouts = raw.timeouts.Select(s => timeoutsDi[s]).ToArr();
            res.nAuthorizationsFromEntrance = raw.nAuthorizationsFromEntrance;
            res.nAuthorizationsFromExit = raw.nAuthorizationsFromExit;
            res.OperatorId = raw.OperatorId;
            res.nPassengersFromEntracePerpetual = raw.nPassengersFromEntracePerpetual;
            res.nPassengersFromExitPerpetual = raw.nPassengersFromExitPerpetual;

            string s = raw.status;
            res.bUserProcessing = s[0] == '1';
            res.bDoorOpen = s[1] == '1';
            res.bFraudOrIntrusion = s[2] == '1';
            res.bTimeout = s[3] == '1';
            res.bTechnicalDefect = s[4] == '1';
            res.bEmergency = s[5] == '1';
            res.bMaintenance = s[6] == '1';
            res.bSideModesForced = s[7] == '1';

            return res;
        }

        static public Either<GetStatusResponseRawError, GetStatusResponseRaw> Parse(object[] o)
        {
            GetStatusResponseRaw res = new();

            if (o.Length != 15)
                return GetStatusResponseRawError.InvalidLength;

            if (!(o[0] is string s))
                return GetStatusResponseRawError.F0;
            else if (s.Length != 8)
                return GetStatusResponseRawError.F0;
            else
                res.status = s;

            if (o[1] is int nAuthorizationsFromEntrance)
                res.nAuthorizationsFromEntrance = nAuthorizationsFromEntrance;
            else
                return GetStatusResponseRawError.F1;

            if (o[2] is int nAuthorizationsFromExit)
                res.nAuthorizationsFromExit = nAuthorizationsFromExit;
            else
                return GetStatusResponseRawError.F2;

            if (o[3] is string doorOperationgMode)
                res.doorOperationgMode = doorOperationgMode;
            else
                return GetStatusResponseRawError.F3;

            if (o[4] is string operatingModeEntrySide)
                res.operatingModeEntrySide = operatingModeEntrySide;
            else
                return GetStatusResponseRawError.F4;

            if (o[5] is string operatingModeExitSide)
                res.operatingModeExitSide = operatingModeExitSide;
            else
                return GetStatusResponseRawError.F5;

            if (o[6] is int nInfractions)
                res.nInfractions = nInfractions;
            else
                return GetStatusResponseRawError.F6;

            if (o[7] is int nTimeouts)
                res.nTimeouts = nTimeouts;
            else
                return GetStatusResponseRawError.F7;

            if (o[8] is int nErrors)
                res.nErrors = nErrors;
            else
                return GetStatusResponseRawError.F8;

            if (o[9] is object[] infractions && nInfractions > 0)
            {
                res.infractions = new string[infractions.Length];
                for (int i = 0; i < infractions.Length; i++)
                {
                    if (infractions[i] is string sInfraction)
                        res.infractions[i] = sInfraction;
                    else
                        return GetStatusResponseRawError.F9;
                }
            }
            else
                return GetStatusResponseRawError.F9;

            if (o[10] is object[] timeouts && nTimeouts > 0)
            {
                res.timeouts = new string[timeouts.Length];
                for (int i = 0; i < timeouts.Length; i++)
                {
                    if (timeouts[i] is string sTimeout)
                        res.timeouts[i] = sTimeout;
                    else
                        return GetStatusResponseRawError.F10;
                }
            }
            else
                return GetStatusResponseRawError.F10;

            if (o[11] is object[] errors && nErrors > 0)
            {
                res.errors = new DictionaryXmlString[errors.Length];
                for (int i = 0; i < errors.Length; i++)
                {
                    if (errors[i] is DictionaryXmlString dict)
                        res.errors[i] = dict;
                    else
                        return GetStatusResponseRawError.F11;
                }
            }
            else
                return GetStatusResponseRawError.F11;

            if (o[12] is string OperatorId)
                res.OperatorId = OperatorId;
            else
                return GetStatusResponseRawError.F12;

            if (o[13] is int nPassengersFromEntracePerpetual)
                res.nPassengersFromEntracePerpetual = nPassengersFromEntracePerpetual;
            else
                return GetStatusResponseRawError.F13;

            if (o[14] is int nPassengersFromExitPerpetual)
                res.nPassengersFromExitPerpetual = nPassengersFromExitPerpetual;
            else
                return GetStatusResponseRawError.F14;

            return res;
        }
    }
}