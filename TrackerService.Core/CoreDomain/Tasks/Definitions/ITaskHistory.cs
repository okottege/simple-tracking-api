using System;

namespace TrackerService.Core.CoreDomain.Tasks.Definitions
{
    public interface ITaskHistory
    {
        string HistoryId { get; }
        HistoryActionType Action { get; }
        DateTime CreatedDate { get; }
        string CreatedBy { get; }
    }
}