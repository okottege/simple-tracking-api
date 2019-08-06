namespace TrackerService.Core.CoreDomain
{
    public interface IServiceContext
    {
        string RequestId { get; }
        string TenantId { get; }
    }
}