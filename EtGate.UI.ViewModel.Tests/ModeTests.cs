using Avalonia.Controls;
using Domain;
using Domain.Services.InService;
using EtGate.UI.ViewModels;
using GateApp;
using Moq;
using System.Reactive.Subjects;
using Xunit;

namespace EtGate.UI.ViewModel.Tests
{
    public class RightViewModelsCreated
    {
        MainWindowViewModel mainVM;

        public RightViewModelsCreated()
        {
            // Arrange
            var mockModeService = new Mock<IModeService>();
            
            mockModeService.Setup(service => service.EquipmentModeObservable).Returns(subj);
            vm = new MainWindowViewModel(mockModeService.Object, new MockInServiceMgrFactory(), new MockNavigationService());
        }

        MainWindowViewModel vm;
        Subject<Mode> subj = new();

        [Fact]
        public void AppBootingPhase()
        {
            Assert.Null(vm.CurrentModeViewModel);

            subj.OnNext(Mode.AppBooting);
            Assert.IsType<AppBootingViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void OOO()
        {
            subj.OnNext(Mode.OOO);
            Assert.IsType<OOOViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void Emergency()
        {
            subj.OnNext(Mode.Emergency);
            Assert.IsType<EmergencyViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void OOS()
        {
            subj.OnNext(Mode.OOS);
            Assert.IsType<OOSViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void Maintenance()
        {
            subj.OnNext(Mode.Maintenance);
            Assert.IsType<MaintenanceViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void InService()
        {
            subj.OnNext(Mode.InService);
            Assert.IsType<InServiceViewModel>(vm.CurrentModeViewModel);
        }
    }

    internal class MockNavigationService : INavigationService
    {
        public ContentControl host { get ; set; }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            
        }
    }

    public class MockInServiceMgr : IInServiceMgr
    {
        public IObservable<InServiceMgr.State> StateObservable => new BehaviorSubject<InServiceMgr.State>(InServiceMgr.State.Unknown);

        public void Dispose()
        {
            
        }

        public Task HaltFurtherValidations()
        {
            return Task.CompletedTask;
        }
    }

    internal class MockInServiceMgrFactory : IInServiceMgrFactory
    {
        public IInServiceMgr Create()
        {
            return new MockInServiceMgr();
        }
    }
}
