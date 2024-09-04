using LanguageExt;

namespace EtGate.Domain.Services;

public interface IDeviceDate
{
    bool SetDate(DateTimeOffset dt);
    Option<DateTimeOffset> GetDate();
}
