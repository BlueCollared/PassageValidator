﻿//using System.Reactive.Linq;
//using System.Reactive.Subjects;
////using ModuleStatus = EtGate.Domain.ModuleStatus;

//namespace EtGate.Devices.Interfaces
//{
//    public abstract class StatusStreamBase<Status> : IDeviceStatus<Status> where Status: ModuleStatus 
//    {
//        //protected Status CurStatus { get; set; } = default;
//        //public bool IsWorking => CurStatus == null ? false : CurStatus.IsAvailable;
//        protected ReplaySubject<Status> statusSubject = new();

//        IObservable<Status> statusObservable_ => statusSubject.AsObservable()
//            //.ObserveOn(SynchronizationContext.Current)
//            ;
//        public IObservable<Status> statusObservable => statusObservable_;
//    }    
//}
