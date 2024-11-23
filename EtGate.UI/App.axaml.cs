using Autofac;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using DummyQrReaderDeviceController;
using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Devices.IER;
using EtGate.Domain;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Gate.Functions;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using EtGate.IER;
using EtGate.UI.ViewModels;
using EtGate.UI.ViewModels.Maintenance;
using EtGate.UI.Views;
using GateApp;
using Horizon.XmlRpc.Client;
using Microsoft.Extensions.DependencyInjection;
using OneOf;
using System;
using System.Reactive.Concurrency;

namespace EtGate.UI;

using ObsAuthEvents = System.IObservable<OneOf.OneOf<EtGate.Domain.Passage.PassageEvts.Intrusion, EtGate.Domain.Passage.PassageEvts.Fraud, EtGate.Domain.Passage.PassageEvts.OpenDoor, EtGate.Domain.Passage.PassageEvts.PassageTimeout, EtGate.Domain.Passage.PassageEvts.AuthroizedPassengerSteppedBack, EtGate.Domain.Passage.PassageEvts.PassageDone>>;

public partial class App : Avalonia.Application
{
    public void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static Autofac.IContainer? Container { get; private set; }

    public void OnFrameworkInitializationCompleted()
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

        DoForValidation(builder);
        DoForQr(builder);
        DoForGate(builder);

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

        builder.RegisterType<ModeServiceLocalAgent>().As<IModeCommandService>().SingleInstance();
        builder.RegisterType<MockContextRepository>().As<IContextRepository>().SingleInstance();
        AutoFacConfig.RegisterViewModels_ExceptRootVM(builder);

        builder.Register<Func<Type, MaintainenaceViewModelBase>>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return viewModelType => (MaintainenaceViewModelBase)componentContext.Resolve(viewModelType);
        });

        var viewModelAssembly = typeof(AgentLoginViewModel).Assembly;
        builder.RegisterAssemblyTypes(viewModelAssembly);
        
        builder.RegisterType<MaintenanceNavigationService>()
           .As<INavigationService>()
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(Func<Type, MaintainenaceViewModelBase>),
               (pi, ctx) => ctx.Resolve<Func<Type, MaintainenaceViewModelBase>>()
           );

        builder.RegisterType<ModeViewModelFactory>().As<IModeViewModelFactory>().SingleInstance();

        builder.RegisterType<ModeMgrFactory>().As<ISubModeMgrFactory>().SingleInstance();

        builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
        builder.RegisterType<NavigationEventManager>().As<INavigationEventManager>().SingleInstance();
        builder.RegisterType<MaintenanceViewFactory>().As<IViewFactory>().SingleInstance();

        builder.RegisterGeneric(typeof(EventBus<>))
       .As(typeof(IMessagePublisher<>))
       .As(typeof(IMessageSubscriber<>))
       .SingleInstance();

        builder.RegisterGeneric(typeof(EventBus<>))
       .As(typeof(IMessagePublisher<>))
       .As(typeof(IMessageSubscriber<>))
       .SingleInstance();


        builder.Register(
            c =>
            {                
                var qr = c.Resolve<IMessageSubscriber<QrReaderStatus>>();
                return new ModeManager(qr, 
                    c.Resolve<ValidationMgr>().StatusStream,
                    c.Resolve<GateMgr>().StatusStream,
                    c.Resolve<ISubModeMgrFactory>(),
                    new EventLoopScheduler()
                    );
            })
            .As<IModeQueryService>()
            .SingleInstance()
            .AsSelf();

        builder.RegisterType<MockPassageManager>().As<IGateInServiceController>();

        Container = builder.Build();

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

    private static void DoForValidation(ContainerBuilder builder)
    {
        builder.RegisterType<DummyOfflineValidation>()
            //.As<IDeviceStatus<OfflineValidationSystemStatus>>()
            .SingleInstance();

        builder.RegisterType<DummyOnlineValidation>()
           //.As<IDeviceStatus<OnlineValidationSystemStatus>>()
           .SingleInstance();

        builder.RegisterType<OfflineValidationSystem>().AsSelf()
            .SingleInstance();
        builder.RegisterType<OnlineValidationSystem>().AsSelf()
            .SingleInstance();
        builder.RegisterType<ValidationMgr>().AsSelf().SingleInstance();
    }

    private static void DoForQr(ContainerBuilder builder)
    {
        builder.RegisterType<DummyQrReaderDeviceController.DummyQrReaderDeviceController>()
          .As<IQrReaderController>()
          //.As<IDeviceStatus<QrReaderStatus>>()
          .SingleInstance();

        builder.RegisterType<QrReaderMgr>()
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(IQrReaderController),
               (pi, ctx) => ctx.Resolve<IQrReaderController>())
           //.WithParameter(
           //    (pi, ctx) => pi.ParameterType == typeof(IDeviceStatus<QrReaderStatus>),
           //    (pi, ctx) => ctx.Resolve<IDeviceStatus<QrReaderStatus>>())
           .As<IQrReaderMgr>()           
           .SingleInstance();
    }

    private static void DoForGate(ContainerBuilder builder)
    {
        string url = "https://www.google.com/"; // TODO: load from configuration
        var xmlRpc = XmlRpcProxyGen.Create<IIERXmlRpcInterface>();
        xmlRpc.Url = url;
        var ierRpc = new IerActive(new IERXmlRpc(xmlRpc, null));
        var ier = new IerController(ierRpc, ierRpc);

        builder.RegisterInstance(ier)
            .As<IGateController>()
            //.As<IDeviceStatus<GateHwStatus>>()
            .As<IDeviceDate>() // TODO: this is WRONG to assume that only IerController implements IDeveiceDate
            .SingleInstance();

        builder.RegisterInstance(new GateMgr.Config { ClockSynchronizerConfig = new ClockSynchronizer.Config { interval = TimeSpan.FromMinutes(5) } })
            .As<GateMgr.Config>()
            .SingleInstance();

        builder.RegisterType<GateMgr>().AsSelf().SingleInstance();
    }

    private void ConfigureServices(ServiceCollection serviceCollection)
    {
    }
}


internal class MockPassageManager : IGateInServiceController
{
    public IObservable<OneOf<IntrusionX, Fraud, OpenDoor, WaitForAuthroization, CloseDoor>> InServiceEventsObservable => throw new NotImplementedException();

    //IObservable<OneOf<Domain.Passage.IdleEvts.Intrusion, Domain.Passage.IdleEvts.Fraud, PassageTimeout, PassageDone>> IPassageManager.IdleStatusObservable => throw new NotImplementedException();

    public ObsAuthEvents Authorize(string ticketId, int nAuthorizations)
    {
        throw new NotImplementedException();
    }

    public bool Authorize(int nAuthorizations, bool bEntry)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
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