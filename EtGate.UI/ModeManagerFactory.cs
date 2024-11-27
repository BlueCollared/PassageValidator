//using Autofac;
//using Domain.Services.Modes;
//using Equipment.Core.Message;
//using EtGate.Domain.Peripherals.Passage;
//using EtGate.Domain;
//using EtGate.Domain.Peripherals.Qr;
//using EtGate.Domain.ValidationSystem;
//using System.Reactive.Concurrency;

//namespace EtGate.UI
//{
//    public class ModeManagerFactory
//    {
//        private readonly Autofac.IContainer c;


//        public ModeManagerFactory(Autofac.IContainer c, 
//            DeviceStatusSubscriber<QrReaderStatus> qr)
//        {
//            this.c = c;
//            this.qr = qr;
//        }

//        DeviceStatusSubscriber<QrReaderStatus> qr;
//        DeviceStatusSubscriber<OfflineValidationSystemStatus> offline;
//        DeviceStatusSubscriber<OnlineValidationSystemStatus> online;
//        DeviceStatusSubscriber<GateHwStatus> gate;
//        IDeviceStatusPublisher<Mode> modePub;
//        IDeviceStatusPublisher<ActiveFunctionalities> activeFuncs;
//        ISubModeMgrFactory modeMgrFactory;
//        IScheduler scheduler;

//        public ModeManager Create()
//        {
//            if (c != null) {
//                if (qr == null)
//                    qr = c.Resolve<DeviceStatusSubscriber<QrReaderStatus>>();
//            }
//            return new ModeManager(qr, );
//        }
//    }
//}
