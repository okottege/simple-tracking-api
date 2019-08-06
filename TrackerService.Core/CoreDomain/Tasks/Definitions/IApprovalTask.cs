namespace TrackerService.Core.CoreDomain.Tasks.Definitions
{
    public interface IApprovalTask : ITask
    {
        bool Resolved { get; }
    }
}