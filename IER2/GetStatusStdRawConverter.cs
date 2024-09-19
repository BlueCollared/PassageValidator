using AutoMapper;

namespace EtGate.IER;

public static class GetStatusStdRawConverter
{
    public static global::Domain.Peripherals.Passage.GateConfiguration ToGateConfiguration(this GetStatusStdRaw src)
    {
        return new global::Domain.Peripherals.Passage.GateConfiguration
        {
            door_mode = src.door_mode.Map(x=>doorsModeMap[x]),
            operatingModeSideA = src.operatingModeSideA.Map(x => sideOperatingModeMap[x]),
            operatingModeSideB = src.operatingModeSideB.Map(x => sideOperatingModeMap[x])
        };
    }
    static global::Domain.Peripherals.Passage.DoorsMode Convert(this DoorsMode src)
    {
        if (doorsModeMap.TryGetValue(src, out global::Domain.Peripherals.Passage.DoorsMode x))
            return x;
        else
            return global::Domain.Peripherals.Passage.DoorsMode.Other;
    }
    static readonly Dictionary<SideOperatingMode, global::Domain.Peripherals.Passage.SideOperatingMode> sideOperatingModeMap = new()
    {
        { SideOperatingMode.Closed, global::Domain.Peripherals.Passage.SideOperatingMode.Closed },
        { SideOperatingMode.Controlled, global :: Domain.Peripherals.Passage.SideOperatingMode.Controlled },
        { SideOperatingMode.Free, global :: Domain.Peripherals.Passage.SideOperatingMode.Free }
    };

    static readonly Dictionary<DoorsMode, global::Domain.Peripherals.Passage.DoorsMode> doorsModeMap = new()
    {
        { DoorsMode.NormallyClosed, global::Domain.Peripherals.Passage.DoorsMode.NormallyClosed },
        { DoorsMode.NormallyOpenedA, global :: Domain.Peripherals.Passage.DoorsMode.NormallyOpenedA },
        { DoorsMode.NormallyOpenedB, global :: Domain.Peripherals.Passage.DoorsMode.NormallyOpenedB },        
    };
}

public class MappingProfile : Profile
{
    static readonly Dictionary<eDoorsStatesMachine, global::Domain.Peripherals.Passage.DoorsStatesMachine> doorsStatesMachineMap = new()
    {
        { eDoorsStatesMachine.NOMINAL, global::Domain.Peripherals.Passage.DoorsStatesMachine.NOMINAL },
        { eDoorsStatesMachine.TECHNICAL_FAILURE, global :: Domain.Peripherals.Passage.DoorsStatesMachine.OOO},
        { eDoorsStatesMachine.EMERGENCY, global :: Domain.Peripherals.Passage.DoorsStatesMachine.EMERGENCY},
        { eDoorsStatesMachine.MAINTENANCE, global :: Domain.Peripherals.Passage.DoorsStatesMachine.MAINTENANCE},
        { eDoorsStatesMachine.LOCKED_OPEN, global :: Domain.Peripherals.Passage.DoorsStatesMachine.LOCKED_OPEN},
        { eDoorsStatesMachine.POWER_DOWN, global :: Domain.Peripherals.Passage.DoorsStatesMachine.POWER_DOWN},
        { eDoorsStatesMachine.EGRESS, global :: Domain.Peripherals.Passage.DoorsStatesMachine.OOO}
    };

    public MappingProfile()
    {
        CreateMap<GetStatusStdRaw, IerNominalMode>();
        CreateMap<GetStatusStdRaw, IerDoors>();
        CreateMap<GetStatusStdRaw, IerErrors>();
        CreateMap<GetStatusStdRaw, global::Domain.Peripherals.Passage.GateOverallStatus>()
            .ForMember(dest => dest.exceptionMode, opt => opt.MapFrom(src =>
                src.exceptionMode.Map(x => doorsStatesMachineMap[x])
            ));
 //       CreateMap<IerErrors, GateErrors>();
    }
}