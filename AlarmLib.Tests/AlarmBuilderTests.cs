using EtGate.AlarmLib;
using Shouldly;
using AH = AlarmLib.AlarmHelper;

namespace AlarmLib.Tests
{
    public enum Mod1Alarms
    {
        AlmNotConnected,
        Alm1Error,
        Alm2Error,
        Alm3Error,
        Alm3Warning
    }

    public enum Mod1MetaAlarms
    {
        MetaAlm1,
        MetaAlm2,
        MetaAlm3,
        MetaStatus
    }

    public class AlarmBuilderTests
    {
        [Fact]
        public void GoodSUTCreation()
        {            
            var builder = new ModuleAlarmMgrBuilder<Mod1Alarms, Mod1MetaAlarms>();
            builder.AddAlarm(Mod1Alarms.AlmNotConnected, AlarmLevel.NotConnected);
            builder.AddAlarm(Mod1Alarms.Alm1Error, AlarmLevel.Error);
            builder.AddAlarm(Mod1Alarms.Alm2Error, AlarmLevel.Error);
            builder.AddAlarm(Mod1Alarms.Alm3Error, AlarmLevel.Error);
            builder.AddAlarm(Mod1Alarms.Alm3Warning, AlarmLevel.Warning);

            builder.AddMetaAlarm(Mod1MetaAlarms.MetaAlm1, AlarmLevel.Warning, new HashSet<Mod1Alarms> { Mod1Alarms.Alm3Error, Mod1Alarms.Alm3Warning },
                AH.Alms(Mod1MetaAlarms.MetaAlm2)
                );
            builder.AddMetaAlarm(Mod1MetaAlarms.MetaAlm2, 
                null,
                AH.NoAlms<Mod1Alarms>(), 
                AH.Alms(Mod1MetaAlarms.MetaAlm3)
                );

            builder.AddMetaAlarm(Mod1MetaAlarms.MetaAlm3,
                null,
                AH.NoAlms<Mod1Alarms>(),
                AH.NoAlms<Mod1MetaAlarms>()
                );

            builder.AddMetaAlarm(Mod1MetaAlarms.MetaStatus,
                null,
                AH.NoAlms<Mod1Alarms>(),
                AH.Alms(Mod1MetaAlarms.MetaAlm1)
                );

            var sut = builder.Build();
            sut.IsRight.ShouldBeTrue();
        }
    }
}