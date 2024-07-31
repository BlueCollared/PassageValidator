using Domain;
using EtGate.UI.ViewModels;
using EtGate.ViewModel;
using GateApp;
using Moq;
using System.Reactive.Subjects;
using Xunit;

namespace EtGate.UI.ViewModel.Tests
{
    public class ModeTests
    {
        MainWindowViewModel mainVM;

        [Fact]
        public void AppBootingPhase()
        {
            // Arrange
            var mockModeService = new Mock<IModeService>();
            var subj = new Subject<Mode>();
            mockModeService.Setup(service => service.EquipmentModeObservable).Returns(subj);

            MainWindowViewModel vm = new MainWindowViewModel(mockModeService.Object);
            Assert.Null(vm.CurrentModeViewModel);
            
            subj.OnNext(Mode.AppBooting);
            Assert.IsType<AppBootingViewModel>(vm.CurrentModeViewModel);

            subj.OnNext(Mode.OOO);
            Assert.IsType<OOOViewModel>(vm.CurrentModeViewModel);

            subj.OnNext(Mode.Emergency);
            Assert.IsType<EmergencyViewModel>(vm.CurrentModeViewModel);

            //subj.OnNext(Mode.InService);
            //Assert.IsType<InServiceViewModel>(vm.CurrentModeViewModel);

            subj.OnNext(Mode.OOS);
            Assert.IsType<OOSViewModel>(vm.CurrentModeViewModel);

            subj.OnNext(Mode.Maintenance);
            Assert.IsType<MaintenanceViewModel>(vm.CurrentModeViewModel);
        }
    }    
}
