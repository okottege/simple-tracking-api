namespace TrackerService.Core.CoreDomain.Tasks
{
    public enum TaskStatus
    {
        Created,
        Ready,
        InProgress,
        Completed,
        Terminated
    }
}
