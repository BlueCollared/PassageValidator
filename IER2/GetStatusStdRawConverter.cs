using AutoMapper;
using Domain.Peripherals.Passage;
using LanguageExt.UnsafeValueAccess;

namespace EtGate.IER;

public static class GetStatusStdRawConverter
{
    static GetStatusStdRawConverter()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        mapper = config.CreateMapper();
    }

    public static GetStatusStdRawComplete To_GetStatusStdRawComplete(this GetStatusStdRaw src)
    {
        GetStatusStdRawComplete result = new GetStatusStdRawComplete();
        result.AuthorizationA = src.AuthorizationA.Value();
        result.AuthorizationB = src.AuthorizationB.Value();
        result.PassageA = src.PassageA.Value();
        result.PassageB = src.PassageB.Value();
        result.door_mode = src.door_mode.Value();
        result.peopleDetectedInsideA = src.peopleDetectedInsideA.Value();
        result.peopleDetectedInsideB = src.peopleDetectedInsideB.Value();
        result.presencesDetectedInsideA = src.presencesDetectedInsideA.Value();
        result.presencesDetectedInsideB = src.presencesDetectedInsideB.Value();
        result.readerLockedSideA = src.readerLockedSideA.Value();
        result.readerLockedSideB = src.readerLockedSideB.Value();
        result.exceptionMode = src.exceptionMode.Value();
        result.stateIfInNominal = src.stateIfInNominal.Value();
        result.customersActive = src.customersActive.Value();
        result.emergencyButton = src.emergencyButton.Value();
        result.entryLockOpen = src.entryLockOpen.Value();
        result.systemPoweredByUPS = src.systemPoweredByUPS.Value();
        result.majorTechnicalFailures = src.majorTechnicalFailures;
        result.minorTechnicalFailures = src.minorTechnicalFailures;
        result.timeouts = src.timeouts.Value();
        result.infractions = src.infractions.Value();
        result.brakeEnabled = src.brakeEnabled.Value();
        result.doorFailure = src.doorFailure.Value();
        result.doorinitialized = src.doorinitialized.Value();
        result.safety_zone = src.safety_zone.Value();
        result.safety_zone_A = src.safety_zone_A.Value();
        result.safety_zone_B = src.safety_zone_B.Value();
        result.doorCurrentMovementOfObstacle = src.doorCurrentMovementOfObstacle.Value();
        result.door_unexpected_motion = src.door_unexpected_motion.Value();

        return result;
    }

    public static GateHwStatus_ To_GateHwStatus(this GetStatusStdRawComplete src)
    {
        return mapper.Map<GateHwStatus_>(src);
    }

    public static IerNominalMode To_NominalMode(this GetStatusStdRawComplete src)
    {
        return mapper.Map<IerNominalMode>(src);
    }

    //public static GateNominalMode
    //{
    //    return new global::Domain.Peripherals.Passage.GateConfiguration
    //    {
    //        door_mode = src.door_mode.Map(x=>doorsModeMap[x]),
    //        operatingModeSideA = src.operatingModeSideA.Map(x => sideOperatingModeMap[x]),
    //        operatingModeSideB = src.operatingModeSideB.Map(x => sideOperatingModeMap[x])
    //    };
    //}
    //static global::Domain.Peripherals.Passage.DoorsMode Convert(this DoorsMode src)
    //{
    //    if (doorsModeMap.TryGetValue(src, out global::Domain.Peripherals.Passage.DoorsMode x))
    //        return x;
    //    else
    //        return global::Domain.Peripherals.Passage.DoorsMode.Other;
    //}
    
    //static readonly Dictionary<SideOperatingMode, global::Domain.Peripherals.Passage.SideOperatingMode> sideOperatingModeMap = new()
    //{
    //    { SideOperatingMode.Closed, global::Domain.Peripherals.Passage.SideOperatingMode.Closed },
    //    { SideOperatingMode.Controlled, global :: Domain.Peripherals.Passage.SideOperatingMode.Controlled },
    //    { SideOperatingMode.Free, global :: Domain.Peripherals.Passage.SideOperatingMode.Free }
    //};

    //static readonly Dictionary<DoorsMode, global::Domain.Peripherals.Passage.DoorsMode> doorsModeMap = new()
    //{
    //    { DoorsMode.NormallyClosed, global::Domain.Peripherals.Passage.DoorsMode.NormallyClosed },
    //    { DoorsMode.NormallyOpenedA, global :: Domain.Peripherals.Passage.DoorsMode.NormallyOpenedA },
    //    { DoorsMode.NormallyOpenedB, global :: Domain.Peripherals.Passage.DoorsMode.NormallyOpenedB },        
    //};
    private static readonly IMapper mapper;
}

public class MappingProfile : Profile
{
    static readonly Dictionary<OverallState, global::Domain.Peripherals.Passage.GatePhysicalState> doorsStatesMachineMap = new()
    {
        { OverallState.NOMINAL, global::Domain.Peripherals.Passage.GatePhysicalState.NOMINAL },
        { OverallState.TECHNICAL_FAILURE, global :: Domain.Peripherals.Passage.GatePhysicalState.OOO},
        { OverallState.EMERGENCY, global :: Domain.Peripherals.Passage.GatePhysicalState.EMERGENCY},
        { OverallState.MAINTENANCE, global :: Domain.Peripherals.Passage.GatePhysicalState.MAINTENANCE},
        { OverallState.LOCKED_OPEN, global :: Domain.Peripherals.Passage.GatePhysicalState.OOO},
        { OverallState.POWER_DOWN, global :: Domain.Peripherals.Passage.GatePhysicalState.OOO},
        { OverallState.EGRESS, global :: Domain.Peripherals.Passage.GatePhysicalState.OOO}
    };

    public MappingProfile()
    {
        CreateMap<GetStatusStdRawComplete, IerNominalMode>();
        CreateMap<GetStatusStdRawComplete, IerDoors>();
        CreateMap<GetStatusStdRawComplete, IerErrors>();
        CreateMap<GetStatusStdRawComplete, global::Domain.Peripherals.Passage.GateHwStatus_>()
            .ForMember(dest => dest.mode, opt => opt.MapFrom(src =>
                doorsStatesMachineMap[src.exceptionMode])
            );
        //CreateMap<GetStatusStdRawComplete, global::Domain.Peripherals.Passage.GateHwStatus_>()
        //    .ForMember(dest => dest.mode, opt => opt.MapFrom(src =>
        //        src.exceptionMode.Map(x => doorsStatesMachineMap[x])
        //    ));
        //CreateMap<IerErrors, GateErrors>();
    }
}