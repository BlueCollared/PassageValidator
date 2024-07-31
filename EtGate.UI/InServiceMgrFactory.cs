using Autofac;
using Domain.Services.InService;

namespace EtGate.UI
{
    public class InServiceMgrFactory : IInServiceMgrFactory
    {
        private readonly ILifetimeScope _scope;

        public InServiceMgrFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public IInServiceMgr Create()
        {
            // Resolve a new instance of InServiceMgr
            return _scope.Resolve<InServiceMgr>();
        }
    }    
}
