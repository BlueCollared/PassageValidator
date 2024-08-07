using Autofac;
using Autofac.Extensions.DependencyInjection;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Domain;
using Domain.InService;
using Domain.Peripherals.Passage;
using Domain.Peripherals.Qr;
using Domain.Services.Modes;
using DummyQrReaderDeviceController;
using EtGate.Domain;
using EtGate.Domain.Services;
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

            // Register the MainWindowViewModel with bPrimary = true
            builder.RegisterType<MainWindowViewModel>()
                .WithParameter(new NamedParameter("bPrimary", true))
                .Named<MainWindowViewModel>("Primary");

            // Register the MainWindowViewModel with bPrimary = false
            builder.RegisterType<MainWindowViewModel>()
                .WithParameter(new NamedParameter("bPrimary", false))
                .Named<MainWindowViewModel>("Secondary");


            builder.RegisterType<OfflineValidationSystem>().AsSelf();
            builder.RegisterType<OnlineValidationSystem>().AsSelf();

            //builder.RegisterType<QrReaderDeviceControllerProxy>()
            builder.RegisterType<DummyQrReaderDeviceController.DummyQrReaderDeviceController>()
              .As<IQrReader>()
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
                   (pi, ctx) => pi.ParameterType == typeof(IQrReader),
                   (pi, ctx) => ctx.Resolve<IQrReader>())
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
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Container.ResolveNamed<MainWindowViewModel>("Primary")
                };

                var secondaryWindow = new SecondaryWindow
                {
                    DataContext = Container.ResolveNamed<MainWindowViewModel>("Secondary")
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
        public IObservable<OneOf<Intrusion, Fraud, PassageInProgress, PassageTimeout, AuthroizedPassengerSteppedBack, PassageDone>> PassageStatusObservable => throw new NotImplementedException();

        public bool Authorize(string ticketId, int nAuthorizations)
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