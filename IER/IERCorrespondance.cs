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


        public static void TransformFromIER(eDoorsStatesMachine mode, CPLCStatus EM)
        {
            TransformFromIER(mode, eSideOperatingModeGate.OP_MODE_SIDE_CLOSED, eSideOperatingModeGate.OP_MODE_SIDE_CLOSED, eIERDoorModes.OP_MODE_BLOCK_CLOSED, EM);
        }

            public static void TransformFromIER(eDoorsStatesMachine mode, eSideOperatingModeGate entryMode, eSideOperatingModeGate exitMode, eIERDoorModes doorMode, CPLCStatus EM )
        {
            switch (mode)
            {
                case eDoorsStatesMachine.NOMINAL:
                    if (doorMode == eIERDoorModes.OP_MODE_BLOCK_CLOSED)
                    {
                        EM.Mode = GlobalEquipmentMode.OutOfService;
                        EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                        EM.EntryMode = GlobalEquipmentEntryMode.Closed;
                        EM.ExitMode = GlobalEquipmentExitMode.Closed;

                    }
                    else
                    {
                        EM.Mode = GlobalEquipmentMode.InService;
                        switch (entryMode)
                        {
                            case eSideOperatingModeGate.OP_MODE_SIDE_FREE:
                                EM.EntryMode = GlobalEquipmentEntryMode.Free;
                                break;
                            case eSideOperatingModeGate.OP_MODE_SIDE_CONTROLLED:
                                EM.EntryMode = GlobalEquipmentEntryMode.Controlled;
                                break;
                            case eSideOperatingModeGate.OP_MODE_SIDE_CLOSED:
                                EM.EntryMode = GlobalEquipmentEntryMode.Closed;
                                break;
                        }
                        switch (exitMode)
                        {
                            case eSideOperatingModeGate.OP_MODE_SIDE_FREE:
                                EM.ExitMode = GlobalEquipmentExitMode.Free;
                                break;
                            case eSideOperatingModeGate.OP_MODE_SIDE_CONTROLLED:
                                EM.ExitMode = GlobalEquipmentExitMode.Controlled;
                                break;
                            case eSideOperatingModeGate.OP_MODE_SIDE_CLOSED:
                                EM.ExitMode = GlobalEquipmentExitMode.Closed;
                                break;
                        }
                        switch (doorMode)
                        {
                            case eIERDoorModes.OP_MODE_BLOCK_CLOSED:
                                EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                                break;
                            case eIERDoorModes.OP_MODE_OPTICAL_B:
                                EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                                break;
                            case eIERDoorModes.OP_MODE_NOB:
                                EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                                break;
                            case eIERDoorModes.OP_MODE_NC:
                                EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                                break;
                            case eIERDoorModes.OP_MODE_OPTICAL_A:
                                EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                                break;
                            case eIERDoorModes.OP_MODE_NOA:
                                EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                                break;
                        }
                    }
                    if (EM.EntryMode == GlobalEquipmentEntryMode.Closed && EM.ExitMode == GlobalEquipmentExitMode.Closed)
                    {
                        EM.Mode = GlobalEquipmentMode.OutOfService;
                        EM.Direction = GlobalEquipmentDirection.None;
                        EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                    }
                    else if (EM.EntryMode==GlobalEquipmentEntryMode.Free || EM.EntryMode==GlobalEquipmentEntryMode.Controlled)
                    {
                        if (EM.ExitMode==GlobalEquipmentExitMode.Free || EM.ExitMode==GlobalEquipmentExitMode.Controlled)
                        {
                            EM.Direction = GlobalEquipmentDirection.BiDirectional;
                        }
                        else
                        {
                            EM.Direction = GlobalEquipmentDirection.Entry;
                        }
                    }
                    else
                    {
                        EM.Direction = GlobalEquipmentDirection.Exit;
                    }
                    break;
                case eDoorsStatesMachine.LOCKED_OPEN:
                    EM.Mode = GlobalEquipmentMode.OutOfService;
                    EM.Direction = GlobalEquipmentDirection.None;
                    EM.EntryMode = GlobalEquipmentEntryMode.Closed;
                    EM.ExitMode = GlobalEquipmentExitMode.Closed;
                    EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                    break;
                case eDoorsStatesMachine.EMERGENCY:
                    EM.Mode = GlobalEquipmentMode.Emergency;
                    EM.Direction = GlobalEquipmentDirection.BiDirectional;
                    EM.EntryMode = GlobalEquipmentEntryMode.Free;
                    EM.ExitMode = GlobalEquipmentExitMode.Free;
                    EM.Aisle = GlobalEquipmentAisle.NormallyOpen;
                    break;
                case eDoorsStatesMachine.MAINTENANCE:
                    EM.Mode = GlobalEquipmentMode.Maintenance;
                    break;
                case eDoorsStatesMachine.TECHNICAL_FAILURE:
                    //To verify what to do in case of POWER_DOWN. Certainly not OK.
                case eDoorsStatesMachine.POWER_DOWN:
                    EM.Mode = GlobalEquipmentMode.OutOfOrder;
                    EM.Direction = GlobalEquipmentDirection.None;
                    EM.EntryMode = GlobalEquipmentEntryMode.Closed;
                    EM.ExitMode = GlobalEquipmentExitMode.Closed;
                    EM.Aisle = GlobalEquipmentAisle.NormallyClosed;
                    break;
                //EGRESS mode not treated. To see what is usage of this.
            }
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
