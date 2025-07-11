﻿using Domain.Services.InService;
using Domain.Services.Modes;
using Equipment.Core.Message;
using EtGate.Domain;
using EtGate.UI.ViewModels;
using Moq;
using System.Reactive.Linq;
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
            var mockModeService = new Mock<IModeManager>();
            //var mockModeQueryService = new Mock<IModeQueryService>();
            //mockModeQueryService.Setup(service => service.EqptModeObservable).Returns(subjMode);

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
            vm = new MainWindowViewModel(new ModeViewModelFactory(mockModeService.Object,
                new Mock<INavigationService>().Object), subjMode, true, true);
        }

        MainWindowViewModel vm;
        //Subject<(Mode, ISubModeMgr)> subjMode = new();
        DeviceStatusSubscriberTest<(Mode, ISubModeMgr)> subjMode = new();

        [Fact]
        public void AppBootingPhase()
        {
            Assert.Null(vm.CurrentModeViewModel);

            subjMode.Publish(DoNothingModeMgr.Create(Mode.AppBooting));
            Assert.IsType<AppBootingViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void OOO()
        {
            subjMode.Publish(DoNothingModeMgr.Create(Mode.OOO));
            Assert.IsType<OOOViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void Emergency()
        {
            subjMode.Publish(DoNothingModeMgr.Create(Mode.Emergency));
            Assert.IsType<EmergencyViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void OOS()
        {
            subjMode.Publish(DoNothingModeMgr.Create(Mode.OOS));
            Assert.IsType<OOSViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void Maintenance()
        {
            subjMode.Publish(DoNothingModeMgr.Create(Mode.Maintenance));
            Assert.IsType<MaintenanceViewModel>(vm.CurrentModeViewModel);
        }

        [Fact]
        public void InService()
        {
            subjMode.Publish((Mode.InService, new DummyIInServiceMgr()));
            Assert.IsType<InServiceViewModel>(vm.CurrentModeViewModel);
        }
    }
    class DummyIInServiceMgr : IInServiceMgr
    {
        public IObservable<InServiceMgr.State> StateObservable => Observable.Empty<InServiceMgr.State>();

        public void Dispose()
        {

        }

        public Task HaltFurtherValidations()
        {
            return Task.CompletedTask;
        }

        public Task Stop(bool bImmediate)
        {
            return Task.CompletedTask;
        }
    }
}
