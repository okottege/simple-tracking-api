namespace TrackerService.Core.CoreDomain.Tasks
{
    public interface IApprovalTask : IAuditableEntity, ITask
    {
        bool Resolved { get; }
    }
}