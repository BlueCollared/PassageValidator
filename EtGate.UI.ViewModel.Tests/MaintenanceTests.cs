using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core;
using Equipment.Core.Message;
using EtGate.Domain;
using EtGate.Domain.Peripherals.Qr;
using EtGate.Domain.Services;
using EtGate.Domain.Services.Modes;
using EtGate.Domain.Services.Qr;
using EtGate.UI.ViewModels;
using EtGate.UI.ViewModels.Maintenance;
using Moq;
using Shouldly;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Xunit;

namespace EtGate.UI.ViewModel.Tests;

public class MaintenanceTests
{
    MainWindowViewModel mainVM;
    MaintenanceNavigationService nav;
    ModeFacade mockModeService;
    Dummy dummy = new();

    DeviceStatusBus<(Mode, ISubModeMgr)> modePub = new();
    //DeviceStatusSubscriberTest<(Mode, bool)> modeSub = new();
    public MaintenanceTests()
    {
        mockModeService = new ModeFacade(PeripheralStatuses_Test.ForTests(),
            new Mock<ISubModeMgrFactory>().Object,
            modePub,
            new Mock<IDeviceStatusPublisher<ActiveFunctionalities>>().Object,
            new EventLoopScheduler()
            );
        nav = new MaintenanceNavigationService(CreateVM, new NavigationEventManager(), mockModeService);
        mainVM = new MainWindowViewModel(new ModeViewModelFactory(mockModeService, nav),
            modePub, true, true);
    }

    [Fact]
    void State0_MaintNavCurrentViewModelIsNullInStart()
    {
        Assert.Null(nav.CurrentViewModel);
        nav.ViewModelStackCopy.ShouldBeEmpty();
    }

    [Fact]
    void State1_SwitchingToMaintenanceAndCallInit()
    {
        mockModeService.SwitchToMaintenance();
        mainVM.CurrentModeViewModel.ShouldBeOfType<MaintenanceViewModel>();

        //((MaintenanceViewModel) mainVM.CurrentModeViewModel).Init();
        nav.CurrentViewModel.ShouldBeOfType<AgentLoginViewModel>();
        nav.CurrentViewModel.IsDisposed.ShouldBeFalse();
    }

    [Fact]
    void BackFromAgentLoginPage()
    {
        State1_SwitchingToMaintenanceAndCallInit();
        var agentLoginVM = nav.CurrentViewModel;

        agentLoginVM.GoBackCommand.Execute(null);
        agentLoginVM.IsDisposed.ShouldBeTrue();

        mainVM.CurrentModeViewModel.ShouldNotBeOfType<MaintenanceViewModel>();
    }

    [Fact]
    void State2_LoginSuccess()
    {
        State1_SwitchingToMaintenanceAndCallInit();

        var agLogVM = (AgentLoginViewModel)nav.CurrentViewModel;
        agLogVM.IsDisposed.ShouldBeFalse();

        agLogVM.LoginCommand.Execute(null);

        nav.CurrentViewModel.ShouldBeOfType<MaintenanceMenuViewModel>();
        //var mainMenuVM = nav.CurrentViewModel;
        agLogVM.IsDisposed.ShouldBeTrue();
    }

    [Fact]
    void State3_SwitchingToLeafPage()
    {
        State2_LoginSuccess();
        var mainMenuVM = (MaintenanceMenuViewModel)nav.CurrentViewModel;

        mainMenuVM.FlapMaintenanceSelectedCommand.Execute(null);
        mainMenuVM.IsDisposed.ShouldBeTrue();

        nav.CurrentViewModel.ShouldBeOfType<FlapMaintenanceViewModel>();
    }

    // TODO: remove it
    //[Fact]
    //void State3_LastVMShouldBeDisposedOnMovingForward()
    //{
    //    State3_SwitchingToLeafPage();

    //    var flapMainVM = (FlapMaintenanceViewModel)nav.CurrentViewModel;
    //    flapMainVM.IsDisposed.ShouldBeFalse();
    //    var stak = nav.ViewModelStackCopy;
    //    stak.Pop();
    //    var lastVM = stak.Peek();
    //    lastVM.IsDisposed.ShouldBeTrue();
    //}

    [Fact]
    void State4_GoBackFromLeafPage()
    {
        State3_SwitchingToLeafPage();
        var stak = nav.ViewModelStackCopy;
        stak.Pop();
        var lastVM = stak.Peek();

        // Act
        nav.CurrentViewModel.GoBackCommand.Execute(null);

        // Assert
        nav.CurrentViewModel.GetType().ShouldBe(lastVM);
        nav.CurrentViewModel.IsDisposed.ShouldBeFalse();
    }

    [Fact]
    void State5_GoBackToNormal()
    {
        State4_GoBackFromLeafPage();
        mainVM.CurrentModeViewModel.ShouldBeOfType<MaintenanceViewModel>();
        nav.CurrentViewModel.GoBackCommand.Execute(null);

        nav.CurrentViewModel.ShouldBeNull();
        nav.ViewModelStackCopy.ShouldBeEmpty();
        //mainVM.CurrentModeViewModel.ShouldNotBeOfType<MaintenanceViewModel>();
    }

    MaintainenaceViewModelBase CreateVM(Type typ)
    {
        Mock<ILoginService> loginService = new();
        var x = new DeviceStatusSubscriberTest<QrReaderStatus>();
        //new Agent { id = "123", name = "abc", opTyp = OpTyp.Supervisor }
        loginService.Setup(service => service.Login(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Agent());
        loginService
            .Setup(service => service.Logout())
            .Verifiable();
        if (typ == typeof(AgentLoginViewModel))
            return new AgentLoginViewModel(loginService.Object, nav);
        else if (typ == typeof(MaintenanceMenuViewModel))
            return new MaintenanceMenuViewModel(
                nav,
                new QrReaderMgr(new Mock<IQrReaderController>().Object, x),
                x
                );
        else if (typ == typeof(FlapMaintenanceViewModel))
            return new FlapMaintenanceViewModel(nav);
        else
            throw new NotImplementedException();
    }


    Subject<Mode> subj = new();
}
