using Domain;
using Domain.Services.InService;
using EtGate.Domain;
using EtGate.Domain.Services;
using EtGate.UI.ViewModels;
using EtGate.UI.ViewModels.Maintenance;
using GateApp;
using Moq;
using Shouldly;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xunit;

namespace EtGate.UI.ViewModel.Tests
{
    public class MaintenanceTests
    {
        MainWindowViewModel mainVM;
        MaintenanceNavigationService nav;
        
        Dummy dummy = new();
        
        IModeService mockModeService = new MockModeService();
        public MaintenanceTests()
        {
            nav = new MaintenanceNavigationService(CreateVM, dummy.Dummy_IViewFactory, mockModeService);            

            mainVM = new MainWindowViewModel(new ModeViewModelFactory(mockModeService, nav), mockModeService, true, true);
        }

        [Fact]
        void State0_MaintNavCurrentViewModelIsNullInStart()
        {
            Assert.Null(nav.CurrentViewModel);
            nav.ViewModelStack.ShouldBeEmpty();
        }

        [Fact]
        void State1_SwitchingToMaintenanceAndCallInit()
        {
            mockModeService.SwitchToMaintenance();
            mainVM.CurrentModeViewModel.ShouldBeOfType<MaintenanceViewModel>();
            
            ((MaintenanceViewModel) mainVM.CurrentModeViewModel).Init(dummy.Dummy_ContentControl);
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

        [Fact]
        void State3_LastVMShouldBeDisposedOnMovingForward()
        {
            State3_SwitchingToLeafPage();

            var flapMainVM = (FlapMaintenanceViewModel)nav.CurrentViewModel;
            flapMainVM.IsDisposed.ShouldBeFalse();
            var stak = nav.ViewModelStack;
            stak.Pop();
            var lastVM = stak.Peek();
            lastVM.IsDisposed.ShouldBeTrue();
        }

        [Fact]
        void State4_GoBackFromLeafPage()
        {            
            State3_SwitchingToLeafPage();
            var stak = nav.ViewModelStack;
            stak.Pop();
            var lastVM = stak.Peek();

            // Act
            nav.CurrentViewModel.GoBackCommand.Execute(null);

            // Assert
            nav.CurrentViewModel.ShouldBeOfType(lastVM.GetType());
            nav.CurrentViewModel.IsDisposed.ShouldBeFalse();
        }

        [Fact]
        void State5_GoBackToNormal()
        {
            State4_GoBackFromLeafPage();
            mainVM.CurrentModeViewModel.ShouldBeOfType<MaintenanceViewModel>();
            nav.CurrentViewModel.GoBackCommand.Execute(null);

            nav.CurrentViewModel.ShouldBeNull();
            nav.ViewModelStack.ShouldBeEmpty();
            mainVM.CurrentModeViewModel.ShouldNotBeOfType<MaintenanceViewModel>();
        }

        MaintainenaceViewModelBase CreateVM(Type typ)
        {
            Mock<ILoginService> loginService = new();
            //new Agent { id = "123", name = "abc", opTyp = OpTyp.Supervisor }
            loginService.Setup(service => service.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Agent());
            loginService
                .Setup(service => service.Logout())
                .Verifiable();
            if (typ == typeof(AgentLoginViewModel))
                return new AgentLoginViewModel(loginService.Object, nav);
            else if (typ == typeof(MaintenanceMenuViewModel))
                return new MaintenanceMenuViewModel(nav, dummy.Dummy_IQrReaderMgr);
            else if (typ == typeof(FlapMaintenanceViewModel))
                return new FlapMaintenanceViewModel(nav);
            else
                throw new NotImplementedException();
        }

        
        Subject<Mode> subj = new();

        private class MockModeService : IModeService
        {
            Subject<Mode> subjMode = new();
            public IObservable<Mode> EquipmentModeObservable => subjMode.AsObservable();

            public ISubModeMgr curModeMgr => throw new NotImplementedException();

            public bool ChangeMode(OpMode mode, TimeSpan timeout)
            {
                return true;
            }

            public Task SwitchOutMaintenance()
            {
                subjMode.OnNext(Mode.OOO);
                return Task.CompletedTask;
            }

            public Task SwitchToMaintenance()
            {
                subjMode.OnNext(Mode.Maintenance);
                return Task.CompletedTask;
            }
        }
    }
}
