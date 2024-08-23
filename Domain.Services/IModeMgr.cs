namespace Domain.Services.InService
{
    public abstract class ISubModeMgr : IDisposable
    {
        public bool IsDisposed { get; protected set; } = false;

        public abstract void Dispose();
        public abstract Task Stop();
    }
}