using System;

namespace TrackerService.Core.CoreDomain.Tasks
{
    public interface ITaskHistory
    {
        string HistoryId { get; }
        HistoryActionType Action { get; }
        DateTime CreatedDate { get; }
        string CreatedBy { get; }
    }
}