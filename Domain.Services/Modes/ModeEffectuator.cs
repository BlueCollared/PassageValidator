using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core.Message;
using System.Reactive.Linq;

namespace EtGate.Domain.Services.Modes
{
    public class ModeEffectuator
    {
        private readonly ISubModeMgrFactory modeMgrFactory;
        private readonly DeviceStatusSubscriber<(Mode mode, bool bImmediate)> modeSub;
        public ISubModeMgr curModeMgr;

        public ModeEffectuator(
            ISubModeMgrFactory modeMgrFactory,
            IDeviceStatusPublisher<(Mode, ISubModeMgr)> modeEffectuatedPub,
            DeviceStatusSubscriber<(Mode, bool)> modeSub)
        {
            this.modeMgrFactory = modeMgrFactory;
            this.modeSub = modeSub;
            //curModeMgr = modeMgrFactory.Create(Mode.AppBooting);

            modeSub.Messages.ForEachAsync(async x => {
                Mode mode = x.Item1;
                bool bImmediate = x.Item2;

                await (curModeMgr?.Stop(bImmediate) ?? Task.CompletedTask);
                curModeMgr?.Dispose();
                
                curModeMgr = modeMgrFactory.Create(mode);
                modeEffectuatedPub?.Publish((mode, curModeMgr));
            });
        }
    }
}
