using Autofac;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Domain;
using Domain.InService;
using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.InService;
using Domain.Services.Modes;
using EtGate.Domain;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Qr;
using EtGate.Domain.Services.Validation;
using EtGate.Domain.ValidationSystem;
using EtGate.QrReader.Proxy;
using EtGate.UI.ViewModels;
using EtGate.UI.ViewModels.Maintenance;
using EtGate.UI.Views;
using GateApp;
using Microsoft.Extensions.DependencyInjection;
using OneOf;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;

namespace EtGate.UI
{
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
            // Register your ViewModels and other services here
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<OfflineValidationSystem>().AsSelf();
            builder.RegisterType<OnlineValidationSystem>().AsSelf();

            builder.RegisterType<QrReaderDeviceControllerProxy>()
              .As<IQrReader>()
              .As<IDeviceStatus<QrReaderStatus>>()
              .SingleInstance();
            //builder.RegisterType<ModeManager>().AsSelf();
            builder.RegisterType<ValidationMgr>().AsSelf().SingleInstance();
            builder.RegisterType<ModeService>().As<IModeService>().SingleInstance();
            builder.RegisterType<MockContextRepository>().As<IContextRepository>().SingleInstance();

            builder.RegisterType<MaintenanceViewModel>().InstancePerDependency();
            builder.RegisterType<MaintenanceMenuViewModel>().InstancePerDependency();
            builder.RegisterType<AgentLoginViewModel>().InstancePerDependency();

            builder.RegisterType<NavigationService>().As<INavigationService>().AsSelf().SingleInstance();
            builder.RegisterType<LoginService>().AsSelf().SingleInstance();

            builder.RegisterType<QrReaderMgr>()
               .WithParameter(
                   (pi, ctx) => pi.ParameterType == typeof(IQrReader),
                   (pi, ctx) => ctx.Resolve<IQrReader>())
               .WithParameter(
                   (pi, ctx) => pi.ParameterType == typeof(IDeviceStatus<QrReaderStatus>),
                   (pi, ctx) => ctx.Resolve<IDeviceStatus<QrReaderStatus>>());

            builder.RegisterType<MockOffline>()
               .As<IDeviceStatus<OfflineValidationSystemStatus>>();

            builder.RegisterType<MockOnline>()
               .As<IDeviceStatus<OnlineValidationSystemStatus>>();

            
            builder.RegisterType<ModeManager>()
            .WithParameter((pi, ctx) =>
                pi.ParameterType == typeof(IPassageManager),
                (pi, ctx) => (object)null) // Inject null for IPassageManager
            .WithParameter((pi, ctx) =>
                pi.ParameterType == typeof(IScheduler),
                (pi, ctx) => DefaultScheduler.Instance); // Inject default scheduler

            builder.RegisterType<MockPassageManager>().As<IPassageManager>();
            builder.RegisterType<MockMmi>().As<IMMI>();

            builder.RegisterType<InServiceMgr>().InstancePerDependency();            
            builder.RegisterType<InServiceMgrFactory>().As<IInServiceMgrFactory>();

            Container = builder.Build();

            var serviceProvider = new AutofacServiceProvider(Container);
            serviceProvider.GetService<NavigationService>()._serviceProvider = serviceProvider;

            //var removeMe = serviceProvider.GetService<AgentLoginViewModel>();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = serviceProvider.GetService<MainWindowViewModel>()
                    //  DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            
        }
    }

    internal class MockMmi : IMMI
    {
        public void IntrusionCleared()
        {
            throw new NotImplementedException();
        }

        public void IntrusionDuringAuthorizedPassage(Intrusion x)
        {
            throw new NotImplementedException();
        }

        public void IntrusionWhenIdle(Intrusion x)
        {
            throw new NotImplementedException();
        }
    }

    internal class MockPassageManager : IPassageManager
    {
        public IObservable<OneOf<Intrusion, Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone>> PassageStatusObservable => throw new NotImplementedException();

        public bool Authorize(string ticketId, int nAuthorizations)
        {
            throw new NotImplementedException();
        }
    }

    public class MockOffline : IDeviceStatus<OfflineValidationSystemStatus>, IValidate
    {
        public override IObservable<OfflineValidationSystemStatus> statusObservable => subject;
        ReplaySubject<OfflineValidationSystemStatus> subject = new();

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }

    public class MockOnline : IDeviceStatus<OnlineValidationSystemStatus>, IValidate
    {
        public override IObservable<OnlineValidationSystemStatus> statusObservable => subject;
        ReplaySubject<OnlineValidationSystemStatus> subject = new();

        public QrCodeValidationResult Validate(QrCodeInfo qrCode)
        {
            throw new NotImplementedException();
        }
    }

    public class MockContextRepository : IContextRepository
    {
        public void SaveMode(OpMode mode)
        {
            //throw new System.NotImplementedException();
        }
    }
}