using IFS2.Equipment.DriverInterface;

namespace IFS2.Equipment.HardwareInterface.IERPLCManager
{
    public static class IERHelper
    {
        public static SideOperatingModes TransformFromChangeModeSideA(ChangeEquipmentMode EM)
        {
            SideOperatingModes _SideOperationModeA = SideOperatingModes.Closed;
            if (EM.Mode == GlobalEquipmentMode.InService)
            {
                switch (EM.EntryMode)
                {
                    case GlobalEquipmentEntryMode.Controlled:
                        if (EM.Direction == GlobalEquipmentDirection.Entry || EM.Direction == GlobalEquipmentDirection.BiDirectional)
                            _SideOperationModeA = SideOperatingModes.Controlled;
                        break;
                    case GlobalEquipmentEntryMode.Free:
                    case GlobalEquipmentEntryMode.FreeControlled:
                        if (EM.Direction == GlobalEquipmentDirection.Entry || EM.Direction == GlobalEquipmentDirection.BiDirectional)
                            _SideOperationModeA = SideOperatingModes.Free;
                        break;
                }
            }
            return _SideOperationModeA;
        }
        public static SideOperatingModes TransformFromChangeModeSideB(ChangeEquipmentMode EM)
        {
            SideOperatingModes _SideOperationModeB = SideOperatingModes.Closed;
            if (EM.Mode == GlobalEquipmentMode.InService)
            {
                switch (EM.ExitMode)
                {
                    case GlobalEquipmentExitMode.Controlled:
                        if (EM.Direction == GlobalEquipmentDirection.Exit || EM.Direction == GlobalEquipmentDirection.BiDirectional)
                            _SideOperationModeB = SideOperatingModes.Controlled;
                        break;
                    case GlobalEquipmentExitMode.Free:
                    case GlobalEquipmentExitMode.FreeControlled:
                        if (EM.Direction == GlobalEquipmentDirection.Exit || EM.Direction == GlobalEquipmentDirection.BiDirectional)
                            _SideOperationModeB = SideOperatingModes.Free;
                        break;
                }
            }
            return _SideOperationModeB;
        }

        public static ePLCState FromMode(GlobalEquipmentMode mode)
        {
            switch (mode)
            {
                case GlobalEquipmentMode.OutOfOrder:
                    return ePLCState.OUTOFORDER;
                case GlobalEquipmentMode.OutOfService:
                    return ePLCState.OUTOFSERVICE;
                case GlobalEquipmentMode.Emergency:
                    return ePLCState.EMERGENCY;
                case GlobalEquipmentMode.InService:
                    return ePLCState.INSERVICE;
                case GlobalEquipmentMode.Maintenance:
                    return ePLCState.MAINTENANCE;
                case GlobalEquipmentMode.StationClosed:
                    return ePLCState.INSERVICE;
            }
            return ePLCState.OUTOFSERVICE;
        }

        public static DoorsMode TransformFromChangeModeDoorsMode(ChangeEquipmentMode EM)
        {
            DoorsMode _doorsMode = DoorsMode.BlockClosed;
            if (EM.Mode == GlobalEquipmentMode.InService)
            {
                if (EM.Aisle == GlobalEquipmentAisle.NormallyClosed)
                {
                    _doorsMode = DoorsMode.Nc;
                }
                else if (EM.Aisle == GlobalEquipmentAisle.NormallyOpen)
                {
                    switch (EM.Direction)
                    {
                        case GlobalEquipmentDirection.Entry:
                            {
                                switch (EM.EntryMode)
                                {
                                    case GlobalEquipmentEntryMode.Controlled:
                                        _doorsMode = Configuration.ReadBoolParameter("ForceDoorinReverseSide", false) ? DoorsMode.Nob : DoorsMode.Noa;
                                        break;
                                    case GlobalEquipmentEntryMode.Free:
                                    case GlobalEquipmentEntryMode.FreeControlled:
                                        _doorsMode = Configuration.ReadBoolParameter("ForceDoorinReverseSide", false) ? DoorsMode.Nob : DoorsMode.Noa;
                                        break;
                                }
                                break;
                            }
                        case GlobalEquipmentDirection.Exit:
                            {
                                switch (EM.ExitMode)
                                {
                                    case GlobalEquipmentExitMode.Controlled:
                                        _doorsMode = Configuration.ReadBoolParameter("ForceDoorinReverseSide", false) ? DoorsMode.Noa : DoorsMode.Nob;
                                        break;
                                    case GlobalEquipmentExitMode.Free:
                                    case GlobalEquipmentExitMode.FreeControlled:
                                        _doorsMode = Configuration.ReadBoolParameter("ForceDoorinReverseSide", false) ? DoorsMode.Noa : DoorsMode.Nob;
                                        break;
                                }
                                break;
                            }
                        case GlobalEquipmentDirection.BiDirectional:
                            {
                                switch (EM.EntryMode)
                                {
                                    case GlobalEquipmentEntryMode.Controlled:
                                        switch (EM.ExitMode)
                                        {
                                            case GlobalEquipmentExitMode.Controlled:
                                                _doorsMode = Configuration.ReadBoolParameter("ForceDoorinEntrySide", true) ? DoorsMode.Noa : DoorsMode.Nob;
                                                break;
                                            case GlobalEquipmentExitMode.Free:
                                            case GlobalEquipmentExitMode.FreeControlled:
                                                _doorsMode = Configuration.ReadBoolParameter("ForceDoorinFreeSide", true) ? DoorsMode.Nob : DoorsMode.Noa;
                                                break;
                                            case GlobalEquipmentExitMode.Closed:
                                                _doorsMode = Configuration.ReadBoolParameter("ForceDoorinReverseSide", false) ? DoorsMode.Nob : DoorsMode.Noa;
                                                break;
                                        }
                                        break;
                                    case GlobalEquipmentEntryMode.Free:
                                    case GlobalEquipmentEntryMode.FreeControlled:
                                        switch (EM.ExitMode)
                                        {
                                            case GlobalEquipmentExitMode.Controlled:
                                                _doorsMode = Configuration.ReadBoolParameter("ForceDoorinFreeSide", true) ? DoorsMode.Noa : DoorsMode.Nob;
                                                break;
                                            case GlobalEquipmentExitMode.Free:
                                            case GlobalEquipmentExitMode.FreeControlled:
                                                _doorsMode = Configuration.ReadBoolParameter("ForceDoorinEntrySide", true) ? DoorsMode.OpticalA : DoorsMode.OpticalB;
                                                break;
                                            case GlobalEquipmentExitMode.Closed:
                                                _doorsMode = Configuration.ReadBoolParameter("ForceDoorinReverseSide", false) ? DoorsMode.Nob : DoorsMode.Noa;
                                                break;
                                        }
                                        break;
                                    case GlobalEquipmentEntryMode.Closed:
                                        switch (EM.ExitMode)
                                        {
                                            case GlobalEquipmentExitMode.Controlled:
                                                _doorsMode = Configuration.ReadBoolParameter("ForceDoorinReverseSide", false) ? DoorsMode.Noa : DoorsMode.Nob;
                                                break;
                                            case GlobalEquipmentExitMode.Free:
                                            case GlobalEquipmentExitMode.FreeControlled:
                                                _doorsMode = Configuration.ReadBoolParameter("ForceDoorinReverseSide", false) ? DoorsMode.Noa : DoorsMode.Nob;
                                                break;
                                        }
                                        break;
                                }
                                break;
                            }
                    }
                }
            }
            return _doorsMode;
        }
    }

    internal class Configuration
    {
        internal static bool ReadBoolParameter(string v1, bool v2)
        {
            throw new NotImplementedException();
        }
    }
}
