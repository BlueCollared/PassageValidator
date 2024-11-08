using LanguageExt;
using System.Reactive.Linq;

namespace EtGate.IER
{
    enum Nothing { Nothing };
    public class IerActive : IIerStatusMonitor, IIerXmlRpc
    {
        // poll the IER continuously, but with a lower priority (if lock to the device can't be acquired,
        // it waits for 10 msec before re-trying
        public IObservable<Either<IERApiError, GetStatusStdRaw>> StatusObservable =>
            // Q: is it fundamentally good? are we sure that if the processing inside `Select` takes more that the 
            // timespan mentioned in .Interval, it would not cause the processing of next element?
            // Ans: yes, checked in sample program
            Observable.Interval(TimeSpan.FromMilliseconds(5000000)) // TODO: correct it
            .Select(_ =>
            {
                // double-check locking for efficiency
                while (true)
                {
                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(lck, ref lockTaken);
                        if (lockTaken)
                        {
                            return worker.GetStatusStd();
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(lck);
                        }
                    }
                }
            })
            .DistinctUntilChanged();

        IIerXmlRpc worker;

        public IerActive(IIerXmlRpc worker)
        {
            this.worker = worker;
        }

        object lck = new();

        public Option<object[]> ApplyUpdate()
        {
            lock (lck)
            {
                return worker.ApplyUpdate();
            }
        }

        public Option<object[]> GetCounter()
        {
            lock (lck)
            {
                return worker.GetCounter();
            }
        }

        public Option<object[]> GetCurrentPassage()
        {
            lock (lck)
            {
                return worker.GetCurrentPassage();
            }
        }

        public Either<IERApiError, DateTime> GetDate()
        {
            lock (lck)
            {
                return worker.GetDate();
            }
        }

        public Option<object[]> GetMotorSpeed()
        {
            lock (lck)
            {
                return worker.GetMotorSpeed();
            }
        }

        public Either<IERApiError, Success> GetSetTempoFlow(TempoFlowConf conf)
        {
            lock (lck)
            {
                return worker.GetSetTempoFlow(conf);
            }
        }

        public Either<IERApiError, IERStatus> GetStatus()
        {
            lock (lck)
            {
                return worker.GetStatus();
            }
        }

        public Either<IERApiError, GetStatusStdRaw> GetStatusStd()
        {
            lock (lck)
            {
                return worker.GetStatusStd();
            }
        }

        public Either<IERApiError, TempoConf> GetTempo()
        {
            lock (lck)
            {
                return worker.GetTempo();
            }
        }

        public Either<IERApiError, IERSWVersion> GetVersion()
        {
            lock (lck)
            {
                return worker.GetVersion();
            }
        }

        public Either<IERApiError, Success> Reboot()
        {
            lock (lck)
            {
                return worker.Reboot();
            }
        }

        public Either<IERApiError, Success> Restart()
        {
            lock (lck)
            {
                return worker.Restart();
            }
        }

        public Either<IERApiError, Success> SetAuthorisation(int nbpassage, int direction)
        {
            lock (lck)
            {
                return worker.SetAuthorisation(nbpassage, direction);
            }
        }

        public Either<IERApiError, Success> SetBuzzerFraud(int volume, int note)
        {
            lock (lck)
            {
                return worker.SetBuzzerFraud(volume, note);
            }
        }

        public Either<IERApiError, Success> SetBuzzerIntrusion(int volume, int note)
        {
            lock (lck)
            {
                return worker.SetBuzzerIntrusion(volume, note);
            }
        }

        public Option<object[]> SetBuzzerMode(int[] param)
        {
            lock (lck)
            {
                return worker.SetBuzzerMode(param);
            }
        }

        public Option<object[]> SetCredentials(string[] param)
        {
            lock (lck)
            {
                return worker.SetCredentials(param);
            }
        }

        public Either<IERApiError, Success> SetDate(DateTime dt, string timezone = "")
        {
            lock (lck)
            {
                return worker.SetDate(dt, timezone);
            }
        }

        public Either<IERApiError, Success> SetEmergency(bool bEnabled)
        {
            lock (lck)
            {
                return worker.SetEmergency(bEnabled);
            }
        }

        public Either<IERApiError, Success> SetMaintenanceMode(bool bEnabled)
        {
            lock (lck)
            {
                return worker.SetMaintenanceMode(bEnabled);
            }
        }

        public Either<IERApiError, Success> SetMode(Option<DoorsMode> doorsMode, Option<SideOperatingMode> entry, Option<SideOperatingMode> exit)
        {
            lock (lck)
            {
                return worker.SetMode(doorsMode, entry, exit);
            }
        }

        public Either<IERApiError, Success> SetMotorSpeed(MotorSpeed param)
        {
            lock (lck)
            {
                return worker.SetMotorSpeed(param);
            }
        }

        public Option<object[]> SetOutputClient(int[] param)
        {
            lock (lck)
            {
                return worker.SetOutputClient(param);
            }
        }

        public Either<IERApiError, Success> SetTempo(TempoConf conf)
        {
            lock (lck)
            {
                return worker.SetTempo(conf);
            }
        }

        public void Start()
        {
            // TODO: we have to be sure that StatusObservable starts emitting only when asked to Here
            // else we might loose notifications
            throw new NotImplementedException();
        }
    }
}
