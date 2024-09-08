namespace Domain.Services.InService
{
    public interface ISubModeMgr : IDisposable
    {
        public abstract Task Stop();
    }

    public class DoNothingModeMgr : ISubModeMgr
    {
        private readonly Mode mode;
        bool IsDisposed;

        public static (Mode, DoNothingModeMgr) Create(Mode mode)
        {
            return (mode, new DoNothingModeMgr(mode));
        }

        public DoNothingModeMgr(Mode mode)
        {
            this.mode = mode;
        }
        public void Dispose()
        {
            IsDisposed = true;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }
    }
}