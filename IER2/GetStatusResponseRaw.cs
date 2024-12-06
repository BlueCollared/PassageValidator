using static IFS2.Equipment.HardwareInterface.IERPLCManager.CIERRpcHelper;

namespace EtGate.IER
{
    //  fields in a .NET struct are automatically assigned default values if they are not explicitly initialized
    internal struct GetStatusResponseRaw
    {
        public string status;
        public int nAuthorizationsFromEntrance;
        public int nAuthorizationsFromExit;
        public string doorOperationgMode;
        public string operatingModeEntrySide;
        public string operatingModeExitSide;
        private int _nInfractions;
        public int nInfractions
        {
            get => _nInfractions; set
            {
                _nInfractions = value;
                infractions = new string[_nInfractions];
            }
        }
        public string[] infractions;

        private int _nTimeouts;
        public int nTimeouts
        {
            get => _nTimeouts; set
            {
                _nTimeouts = value;
                timeouts = new string[_nTimeouts];
            }
        }
        public string[] timeouts;

        private int _nErrors;
        public DictionaryXmlString[] errors;

        public int nErrors
        {
            get => _nErrors; set
            {
                _nErrors = value;
                errors = new DictionaryXmlString[_nErrors];
            }
        }

        public string OperatorId;
        public int nPassengersFromEntracePerpetual;
        public int nPassengersFromExitPerpetual;
    }
    /*
    internal struct GetStatusResponseLittleProcessed
    {
        internal DoorsMode doorMode;
        internal SideOperatingModes opModeEntry;
        internal SideOperatingModes opModeExit;
        internal Arr<eInfractions> infractions;
        internal Arr<ePassageTimeouts> timeouts;
        internal int nAuthorizationsFromEntrance;
        internal int nAuthorizationsFromExit;
        internal bool bUserProcessing;
        internal bool bDoorOpen;
        internal bool bFraudOrIntrusion;
        internal bool bEmergency;
        internal bool bMaintenance;
        internal bool bOutOfService;
        internal bool bInService;
        internal bool bTechnicalDefect;
        internal bool bTimeout;
        internal bool bSideModesForced;
        internal string OperatorId;
        internal int nPassengersFromEntracePerpetual;
        internal int nPassengersFromExitPerpetual;
    }
    */

}
