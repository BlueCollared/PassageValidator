using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using DummyQrReaderDeviceController;
using EtGate.Devices.IER;
using EtGate.Domain;
//using EtGate.Domain.Passage.IdleEvts;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Gate;
using EtGate.Domain.Services.Gate.Functions;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using EtGate.IER;
using EtGate.UI.ViewModels;
using EtGate.UI.Views;
using GateApp;
using Horizon.XmlRpc.Client;
using IFS2.Equipment.HardwareInterface.IERPLCManager;
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

    public static Autofac.IContainer? Container { get; private set; }

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

        builder.RegisterType<ModeService>().As<IModeCommandService>().SingleInstance();
        builder.RegisterType<MockContextRepository>().As<IContextRepository>().SingleInstance();
        AutoFacConfig.RegisterViewModels_ExceptRootVM(builder);

        builder.Register<Func<Type, MaintainenaceViewModelBase>>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return viewModelType => (MaintainenaceViewModelBase)componentContext.Resolve(viewModelType);
        });

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
               (pi, ctx) => pi.ParameterType == typeof(IModeCommandService),
               (pi, ctx) => ctx.Resolve<IModeCommandService>()
           ).SingleInstance();

        builder.RegisterType<ModeViewModelFactory>().As<IModeViewModelFactory>().SingleInstance();

        builder.RegisterType<ModeMgrFactory>().As<IModeMgrFactory>().SingleInstance();

        builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();
        builder.RegisterType<MaintenanceViewFactory>().As<IViewFactory>().SingleInstance();

        builder.RegisterType<ModeManager>()
            .WithParameter(new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(IPassageManager),
                (pi, ctx) => ctx.ResolveOptional<IPassageManager>()))
            .WithParameter(new ResolvedParameter(
                (pi, ctx) => pi.ParameterType == typeof(IScheduler),
                (pi, ctx) => ctx.Resolve<IScheduler>()))
            .As<IModeQueryService>()
            .SingleInstance()
            .AsSelf();

        builder.RegisterType<MockPassageManager>().As<IPassageManager>();

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
            .As<IDeviceStatus<OfflineValidationSystemStatus>>()
            .SingleInstance();

        builder.RegisterType<DummyOnlineValidation>()
           .As<IDeviceStatus<OnlineValidationSystemStatus>>()
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
          .As<IDeviceStatus<QrReaderStatus>>()
          .SingleInstance();

        builder.RegisterType<QrReaderMgr>()
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(IQrReaderController),
               (pi, ctx) => ctx.Resolve<IQrReaderController>())
           .WithParameter(
               (pi, ctx) => pi.ParameterType == typeof(IDeviceStatus<QrReaderStatus>),
               (pi, ctx) => ctx.Resolve<IDeviceStatus<QrReaderStatus>>())
           .As<IQrReaderMgr>()
           .SingleInstance();
    }

    private static void DoForGate(ContainerBuilder builder)
    {
        string url = ""; // TODO: load from configuration
        var xmlRpc = XmlRpcProxyGen.Create<IIERXmlRpcInterface>();
        xmlRpc.Url = url;
        var ierRpc = new IerActive(new IERXmlRpc(xmlRpc, null));
        var ier = new IerController(ierRpc, ierRpc);

        builder.RegisterInstance(ier)
            .As<IGateController>()
            .As<IDeviceStatus<GateHwStatus>>()
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


internal class MockPassageManager : IPassageManager
{
    //IObservable<OneOf<Domain.Passage.IdleEvts.Intrusion, Domain.Passage.IdleEvts.Fraud, PassageTimeout, PassageDone>> IPassageManager.IdleStatusObservable => throw new NotImplementedException();

    public ObsAuthEvents Authorize(string ticketId, int nAuthorizations)
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