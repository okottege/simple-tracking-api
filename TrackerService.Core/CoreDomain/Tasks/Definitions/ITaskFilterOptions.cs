namespace TrackerService.Core.CoreDomain.Tasks.Definitions
{
    public interface ITaskFilterOptions
    {
        string CreatedById { get; }
        string Status { get; }
        string AssignedToId { get; }
    }
}