using System;
using TrackerService.Core.CoreDomain.Definitions;

namespace TrackerService.Services.DataObjects.Tasks
{
    public class TaskListItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public IDomainUser CreatedBy { get; set; }
    }
}
