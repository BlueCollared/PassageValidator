using Domain;
using Domain.Services.InService;
using EtGate.UI.ViewModels;
using GateApp;
using Moq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xunit;
using static Domain.Services.InService.InServiceMgr;

namespace EtGate.UI.ViewModel.Tests
{
    public class RightViewModelsCreated
    {
        MainWindowViewModel mainVM;

        public RightViewModelsCreated()
        {
            // Arrange
            var mockModeService = new Mock<IModeService>();
            
            mockModeService.Setup(service => service.EquipmentModeObservable).Returns(subjMode);

            var InServiceMgrStub = new Mock<IInServiceMgr>();
            InServiceMgrStub.Setup(x => x.StateObservable).Returns(
                Observable.Empty<State>()
                //new BehaviorSubject<InServiceMgr.State>(InServiceMgr.State.Unknown)
                );
            InServiceMgrStub.Setup(x => x.HaltFurtherValidations()).Returns(Task.CompletedTask);

            //// Create mock for IInServiceMgrFactory
            //var InServiceMgrFactoryStub = new Mock<IInServiceMgrFactory>();
            //InServiceMgrFactoryStub.Setup(x => x.Create()).Returns(InServiceMgrStub.Object);

            //vm = new MainWindowViewModel(mockModeService.Object, new MockInServiceMgrFactory(), new Mock<INavigationService>().Object);
            vm = new MainWindowViewModel(mockModeService.Object,                 
                new Mock<INavigationService>().Object, true, true);
        }

        MainWindowViewModel vm;
        Subject<Mode> subjMode = new();

        [Fact]
        public void AppBootingPhase()
        {
            Assert.Null(vm.CurrentModeViewModel);

            subjMode.OnNext(Mode.AppBooting);
            Assert.IsType<AppBootingViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void OOO()
        {
            subjMode.OnNext(Mode.OOO);
            Assert.IsType<OOOViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void Emergency()
        {
            subjMode.OnNext(Mode.Emergency);
            Assert.IsType<EmergencyViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void OOS()
        {
            subjMode.OnNext(Mode.OOS);
            Assert.IsType<OOSViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void Maintenance()
        {
            subjMode.OnNext(Mode.Maintenance);
            Assert.IsType<MaintenanceViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void InService()
        {
            subjMode.OnNext(Mode.InService);
            Assert.IsType<InServiceViewModel>(vm.CurrentModeViewModel);
        }
    }
}
