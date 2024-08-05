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

            mainVM = new MainWindowViewModel(mockModeService, new Mock<IInServiceMgrFactory>().Object, nav);
        }

        [Fact]
        void Test()
        {
            Assert.Null(nav.CurrentViewModel);
            {
                mockModeService.SwitchToMaintenance();
                //Assert.Equal<Type>(mainVM.CurrentModeViewModel.GetType(), typeof(MaintenanceViewModel));
                mainVM.CurrentModeViewModel.ShouldBeOfType<MaintenanceViewModel>();
            }
            {
                ((MaintenanceViewModel)mainVM.CurrentModeViewModel).Init(dummy.Dummy_ContentControl);
                nav.CurrentViewModel.ShouldBeOfType<AgentLoginViewModel>();
            }
            {
                var agLogVM = (AgentLoginViewModel)nav.CurrentViewModel;
                agLogVM.IsDisposed.ShouldBeFalse();

                agLogVM.LoginCommand.Execute(null);
                
                nav.CurrentViewModel.ShouldBeOfType<MaintenanceMenuViewModel>();
                var mainMenuVM = nav.CurrentViewModel;
                agLogVM.IsDisposed.ShouldBeTrue();

                mainMenuVM.GoBackCommand.Execute(null);
                mainMenuVM.IsDisposed.ShouldBeTrue();

            }
            {                

            }
            

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
            else
                throw new NotImplementedException();
        }

        
        Subject<Mode> subj = new();

        private class MockModeService : IModeService
        {
            Subject<Mode> subjMode = new();
            public IObservable<Mode> EquipmentModeObservable => subjMode.AsObservable();

            public bool ChangeMode(OpMode mode, TimeSpan timeout)
            {
                return true;
            }

            public void SwitchOutMaintenance()
            {
                subjMode.OnNext(Mode.OOO);
            }

            public void SwitchToMaintenance()
            {
                subjMode.OnNext(Mode.Maintenance);
            }
        }
    }
}
