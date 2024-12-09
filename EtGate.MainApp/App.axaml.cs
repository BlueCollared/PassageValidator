using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Equipment.Core;
using EtGate.DependencyInjection;
using EtGate.UI;
using EtGate.UI.ViewModels;
using EtGate.UI.Views;
using System.IO;
using System.Text.Json;

namespace EtGate.MainApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        const string PrimaryEntry = nameof(PrimaryEntry);
        const string PrimaryExit = nameof(PrimaryExit);
        const string SecondaryExit = nameof(SecondaryExit);
        const string SecondaryEntry = nameof(SecondaryEntry);

        const string bPrimary = "bPrimary";
        const string bEntry = "bEntry";


        public
            //static 
            Autofac.IContainer? Container
        { get; private set; }

        public override void OnFrameworkInitializationCompleted()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var desktop = (IClassicDesktopStyleApplicationLifetime)ApplicationLifetime;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var args = desktop.Args;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            string configFileName = args[0];
            string fullFileName = Path.Combine(Directory.GetCurrentDirectory(), configFileName);            
            string confJson = File.ReadAllText(fullFileName);

            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.WriteIndented = true;
            AppConfig conf = SerializeUtil.JsonDeserialize<AppConfig>(confJson, jsonOptions);
            var builder = new ContainerBuilder();

            builder.RegisterModule(new GateModule(conf.gate));
            builder.RegisterModule(new QrModule(conf.qr));
            builder.RegisterModule(new ValidationModule(conf.validation));
            builder.RegisterModule(new ModeModule());
            builder.RegisterModule(new TemporaryModule(conf.OtherConf));

            DepBuilder.Do(builder);

            DepBuilder.Container = Container = builder.Build();

            //if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
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
    }
}