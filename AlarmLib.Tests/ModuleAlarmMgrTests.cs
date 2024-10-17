using EtGate.AlarmLib;
using System.Diagnostics;
using AH = AlarmLib.AlarmHelper;

namespace AlarmLib.Tests
{
    public class ModuleAlarmMgrTests
    {
        ModuleAlarmMgr<Mod1Alarms, Mod1MetaAlarms> sut;        

        public ModuleAlarmMgrTests()
        {
            var builder = new ModuleAlarmMgrBuilder<Mod1Alarms, Mod1MetaAlarms>();
            builder.AddAlarm(Mod1Alarms.AlmNotConnected, AlarmLevel.NotConnected);
            builder.AddAlarm(Mod1Alarms.Alm1Error, AlarmLevel.Error);
            builder.AddAlarm(Mod1Alarms.Alm2Error, AlarmLevel.Error);
            builder.AddAlarm(Mod1Alarms.Alm3Error, AlarmLevel.Error);
            builder.AddAlarm(Mod1Alarms.Alm3Warning, AlarmLevel.Warning);            

            builder.AddMetaAlarm(Mod1MetaAlarms.MetaAlm1, AlarmLevel.Warning, 
                AH.Alms(Mod1Alarms.Alm3Error, Mod1Alarms.Alm3Warning),
                AH.NoAlms<Mod1MetaAlarms>());

            builder.AddMetaAlarm(Mod1MetaAlarms.MetaStatus, null,
                AH.Alms(Mod1Alarms.AlmNotConnected, Mod1Alarms.Alm1Error, Mod1Alarms.Alm2Error),
                AH.Alms(Mod1MetaAlarms.MetaAlm1)
                );

            builder.Build().Match(r => sut = r, _ => { });
        }

        [Fact]
        void Test1()
        {
            sut.all.Subscribe(x => { 
                Debug.WriteLine(x);
            });
            sut.RaiseAlarm(Mod1Alarms.Alm3Error);
            sut.RaiseAlarm(Mod1Alarms.Alm3Warning);
            //sut.ClearAlarm(Mod1Alarms.Alm3Error);
            //sut.ClearAlarm(Mod1Alarms.Alm3Warning);
            sut.RaiseAlarm(Mod1Alarms.Alm1Error);
            sut.RaiseAlarm(Mod1Alarms.Alm2Error);
            sut.RaiseAlarm(Mod1Alarms.AlmNotConnected);
            sut.RaiseAlarm(Mod1Alarms.Alm3Error);
            sut.ClearAlarm(Mod1Alarms.AlmNotConnected);

            sut.ClearAlarm(Mod1Alarms.Alm1Error);
            sut.ClearAlarm(Mod1Alarms.Alm2Error);

            sut.ClearAlarm(Mod1Alarms.Alm3Error);
            sut.ClearAlarm(Mod1Alarms.Alm3Warning);
        }
    }
}