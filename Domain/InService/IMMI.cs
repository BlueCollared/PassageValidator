using Domain.Peripherals.Passage;

namespace Domain.InService
{
    public interface IMMI
    {
        void IntrusionWhenIdle(Intrusion x);
        void IntrusionDuringAuthorizedPassage(Intrusion x);
        void IntrusionCleared();
    }
}