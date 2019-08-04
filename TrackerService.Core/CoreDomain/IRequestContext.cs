namespace TrackerService.Core.CoreDomain
{
    public interface IRequestContext
    {
        string TenantId { get; }
        string RequestId { get; }
    }
}