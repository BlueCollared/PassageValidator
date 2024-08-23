using Autofac;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using DummyQrReaderDeviceController;
using EtGate.Domain;
using EtGate.Domain.Passage.IdleEvts;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using EtGate.UI.ViewModels;
using EtGate.UI.Views;
using GateApp;
using Microsoft.Extensions.DependencyInjection;
using OneOf;
using System;
using System.Reactive.Concurrency;

namespace EtGate.UI;

using ObsAuthEvents = System.IObservable<OneOf.OneOf<EtGate.Domain.Passage.PassageEvts.Intrusion, EtGate.Domain.Passage.PassageEvts.Fraud, EtGate.Domain.Passage.PassageEvts.OpenDoor, EtGate.Domain.Passage.PassageEvts.PassageTimeout, EtGate.Domain.Passage.PassageEvts.AuthroizedPassengerSteppedBack, EtGate.Domain.Passage.PassageEvts.PassageDone>>;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static Autofac.IContainer Container { get; private set; }

    public override void OnFrameworkInitializationCompleted()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var builder = new ContainerBuilder();
        builder.Populate(serviceCollection);
        builder.RegisterInstance(DefaultScheduler.Instance).As<IScheduler>();

        const string PrimaryEntry = nameof(PrimaryEntry);
        const string PrimaryExit = nameof(PrimaryExit);
        const string SecondaryExit = nameof(SecondaryExit);
        const string SecondaryEntry = nameof(SecondaryEntry);

        const string bPrimary = "bPrimary";
        const string bEntry = "bEntry";

        
        builder.RegisterType<MainWindowViewModel>()
            .WithParameter(new NamedParameter(bPrimary, true))
            .WithParameter(new NamedParameter(bEntry, true))
            .Named<MainWindowViewModel>(PrimaryEntry);

        builder.RegisterType<MainWindowViewModel>()
            .WithParameter(new NamedParameter(bPrimary, false))
            .WithParameter(new NamedParameter(bEntry, true))
            .Named<MainWindowViewModel>(SecondaryEntry);

        builder.RegisterType<MainWindowViewModel>()
            .WithParameter(new NamedParameter(bPrimary, true))
            .WithParameter(new NamedParameter(bEntry, false))
            .Named<MainWindowViewModel>(PrimaryExit);

        builder.RegisterType<MainWindowViewModel>()
            .WithParameter(new NamedParameter(bPrimary, false))
            .WithParameter(new NamedParameter(bEntry, false))
            .Named<MainWindowViewModel>(SecondaryExit);

        builder.RegisterType<OfflineValidationSystem>().AsSelf();
        builder.RegisterType<OnlineValidationSystem>().AsSelf();

        //builder.RegisterType<QrReaderDeviceControllerProxy>()
        builder.RegisterType<DummyQrReaderDeviceController.DummyQrReaderDeviceController>()
          .As<IQrReaderController>()
          .As<IDeviceStatus<QrReaderStatus>>()
          .SingleInstance();
        //builder.RegisterType<ModeManager>().AsSelf();
        builder.RegisterType<ValidationMgr>().AsSelf().SingleInstance();
        builder.RegisterType<ModeService>().As<IModeService>().SingleInstance();
        builder.RegisterType<MockContextRepository>().As<IContextRepository>().SingleInstance();
        AutoFacConfig.RegisterViewModels_ExceptRootVM(builder);

        builder.Register<Func<Type, MaintainenaceViewModelBase>>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return viewModelType => (MaintainenaceViewModelBase)componentContext.Resolve(viewModelType);
        });

        //builder.RegisterType<ViewModelFactory>().AsSelf();
        builder.RegisterType<MaintenanceNavigationService>()
           .As<INavigationService>()
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(Func<Type, MaintainenaceViewModelBase>),
               (pi, ctx) => ctx.Resolve<Func<Type, MaintainenaceViewModelBase>>()
           )
           .WithParameter(
                (pi, _) => pi.ParameterType == typeof(Func<Type, UserControl>),
               (_, ctx) => 
                   ctx.Resolve<IViewFactory>()
            )
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(IModeService),
               (pi, ctx) => ctx.Resolve<IModeService>()
           ).SingleInstance();

        builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();

        builder.RegisterType<QrReaderMgr>()
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(IQrReaderController),
               (pi, ctx) => ctx.Resolve<IQrReaderController>())
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(IDeviceStatus<QrReaderStatus>),
               (pi, ctx) => ctx.Resolve<IDeviceStatus<QrReaderStatus>>())
           .As<IQrReaderMgr>()
           .SingleInstance();

        builder.RegisterType<DummyOfflineValidation>()
           .As<IDeviceStatus<OfflineValidationSystemStatus>>();

        builder.RegisterType<DummyOnlineValidation>()
           .As<IDeviceStatus<OnlineValidationSystemStatus>>();

        builder.RegisterType<MaintenanceViewFactory>().As<IViewFactory>().SingleInstance();
        
        builder.RegisterType<ModeManager>()
        .WithParameter((pi, ctx) =>
            pi.ParameterType == typeof(IPassageManager),
            (pi, ctx) => (object)null) // Inject null for IPassageManager
        .WithParameter((pi, ctx) =>
            pi.ParameterType == typeof(IScheduler),
            (pi, ctx) => DefaultScheduler.Instance); // Inject default scheduler

        builder.RegisterType<MockPassageManager>().As<IPassageManager>();

        //builder.RegisterType<InServiceMgr>().InstancePerDependency();            
        //builder.RegisterType<InServiceMgrFactory>().As<IInServiceMgrFactory>().SingleInstance();

        Container = builder.Build();
        
        // TODO: change it for entry-only/exit-only. also for reverse configuration
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Container.ResolveNamed<MainWindowViewModel>(PrimaryEntry)
            };

            var secondaryWindow = new SecondaryWindow
            {
                DataContext = Container.ResolveNamed<MainWindowViewModel>(SecondaryExit)
            };
            secondaryWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(ServiceCollection serviceCollection)
    {
        
    }
}

internal class MockPassageManager : IPassageManager
{
    public IObservable<OneOf<Intrusion, Fraud, PassageTimeout, PassageDone>> IdleStatusObservable => throw new NotImplementedException();

    public ObsAuthEvents Authorize(string ticketId, int nAuthorizations)
    {
        throw new NotImplementedException();
    }
}


public class MockContextRepository : IContextRepository
{
    public void SaveMode(global::Domain.OpMode mode)
    {
        throw new NotImplementedException();
    }
}