namespace EtGate.Domain.Services.Gate;

public enum Mode { 
    EntryOnly_EntryFree,
    EntryOnly_EntryControlled,
    ExitOnly_ExitFree,
    ExitOnly_ExitControlled,
    BiDi_EntryFree_ExitFree,
    BiDi_EntryFree_ExitControlled,
    BiDi_EntryControlled_ExitFree,
    BiDi_EntryControlled_ExitControlled,
};

public record GateOperationConfig
    (Mode mode, bool bNormallyClosed, FlapOpeningModeApplicableOnlyOnNormallyOpen flapOpeningMode)
{
    private GateOperationConfig() : 
        this(Mode.BiDi_EntryControlled_ExitControlled, true, FlapOpeningModeApplicableOnlyOnNormallyOpen.NA)
    {}

    GateOperationConfig CreateNormallyClosed(Mode mode)=> this with { bNormallyClosed = true, mode = mode };
    GateOperationConfig CreateNormallyOpen(Mode mode, FlapOpeningModeApplicableOnlyOnNormallyOpen flapOpeningMode)
        => this with { bNormallyClosed = false, mode = mode, flapOpeningMode =flapOpeningMode };
}

public enum FlapOpeningModeApplicableOnlyOnNormallyOpen { A, B, NA };

public interface IGateModeController
{
    bool SetOOS();
    bool SetEmergency();
    bool SetMaintenance();
    bool SetNormalMode(GateOperationConfig config);
}