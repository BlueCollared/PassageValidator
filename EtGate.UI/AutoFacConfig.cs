using Autofac;
using EtGate.UI.ViewModels;
using System;
using System.Linq;
using System.Reflection;

public static class AutoFacConfig
{
    public static void RegisterViewModels_ExceptRootVM(ContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var viewModelType = typeof(ViewModelBase);

        var viewModelTypes = assembly.GetTypes()
                                     .Where(t => t.IsSubclassOf(viewModelType) && !t.IsAbstract)
                                     .Where(t => t != typeof(MainWindowViewModel))
                                     .ToList();

        foreach (var vmType in viewModelTypes)
            builder.RegisterType(vmType)
                   .InstancePerDependency()
                   .OnRelease(viewModel => (viewModel as IDisposable)?.Dispose());
    }
}
