namespace Domain.InService
{
    public interface IValidationMgr
    {
        bool bWorking { get; }
        bool Validate();
    }
}