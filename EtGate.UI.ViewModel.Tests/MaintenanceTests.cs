//using Avalonia.Controls;
//using Domain;
//using Domain.Services.InService;
//using EtGate.UI.ViewModels;
//using GateApp;
//using Moq;
//using System.Reactive.Subjects;
//using Xunit;

//namespace EtGate.UI.ViewModel.Tests
//{
//    public class MaintenanceTests
//    {
//        MainWindowViewModel mainVM;

//        public MaintenanceTests()
//        {
//            // Arrange
//            var mockModeService = new Mock<IModeService>();
            
//            mockModeService.Setup(service => service.EquipmentModeObservable).Returns(subj);
//            vm = new MainWindowViewModel(mockModeService.Object, new MockInServiceMgrFactory(), new Mock<INavigationService>().Object);
//        }

//        MainWindowViewModel vm;
//        Subject<Mode> subj = new();

//        [Fact]
//        public void AppBootingPhase()
//        {
//            Assert.Null(vm.CurrentModeViewModel);

//            subj.OnNext(Mode.AppBooting);
//            Assert.IsType<AppBootingViewModel>(vm.CurrentModeViewModel);
//        }

//        [Fact]
//        public void OOO()
//        {
//            subj.OnNext(Mode.OOO);
//            Assert.IsType<OOOViewModel>(vm.CurrentModeViewModel);
//        }

//        [Fact]
//        public void Emergency()
//        {
//            subj.OnNext(Mode.Emergency);
//            Assert.IsType<EmergencyViewModel>(vm.CurrentModeViewModel);
//        }

//        [Fact]
//        public void OOS()
//        {
//            subj.OnNext(Mode.OOS);
//            Assert.IsType<OOSViewModel>(vm.CurrentModeViewModel);
//        }

//        [Fact]
//        public void Maintenance()
//        {
//            subj.OnNext(Mode.Maintenance);
//            Assert.IsType<MaintenanceViewModel>(vm.CurrentModeViewModel);
//        }

//        [Fact]
//        public void InService()
//        {
//            subj.OnNext(Mode.InService);
//            Assert.IsType<InServiceViewModel>(vm.CurrentModeViewModel);
//        }
//    }


//}
