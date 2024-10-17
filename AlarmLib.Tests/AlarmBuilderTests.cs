using EtGate.AlarmLib;
using Shouldly;

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
        MetaStatus
    }

    public class AlarmBuilderTests
    {
        //private const int MOD1ID = 1;

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
                new HashSet<Mod1MetaAlarms> { });

            var sut = builder.Build();
            sut.IsRight.ShouldBeTrue();
        }
    }
}