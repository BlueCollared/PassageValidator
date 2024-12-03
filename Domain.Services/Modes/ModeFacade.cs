using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core;
using Equipment.Core.Message;
using System.Reactive.Concurrency;

namespace EtGate.Domain.Services.Modes;

public class ModeFacade : IModeManager
{
    ModeEvaluator evaluator;
    ModeEffectuator effectuator;

    Mode CurMode {get;  set;} // TODO: remove it. nobody is using it

    public OpMode ModeDemanded { get => evaluator.ModeDemanded; set => evaluator.ModeDemanded = value; }

    public Task SwitchOutMaintenance()
    {
        evaluator.SwitchOutMaintenance();
        return Task.CompletedTask;
    }

    public async Task SwitchToMaintenance()
    {
        await evaluator.SwitchToMaintenance();            
    }

    DeviceStatusBus<(Mode, bool)> busInternal = new();
    
    public ModeFacade(
        PeripheralStatuses peripheralStatuses,
        ISubModeMgrFactory subModeMgrFactory,
        IDeviceStatusPublisher<(Mode, ISubModeMgr)> modePub,
        IDeviceStatusPublisher<ActiveFunctionalities> activeFuncsPub,
        IScheduler scheduler,
        int timeToCompleteAppBoot_InSeconds = ModeEvaluator.DEFAULT_TimeToCompleteBoot_InSeconds)
    {
        busInternal.Messages.Subscribe(msg => { CurMode = msg.Item1; });
        evaluator = new ModeEvaluator(peripheralStatuses, busInternal, activeFuncsPub, scheduler, timeToCompleteAppBoot_InSeconds);
        effectuator = new ModeEffectuator(subModeMgrFactory, modePub, busInternal);
    }
}