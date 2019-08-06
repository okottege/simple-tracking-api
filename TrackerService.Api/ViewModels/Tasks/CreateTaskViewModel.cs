using System;
using System.Collections.Generic;
using TrackerService.Core.CoreDomain.Tasks;

namespace TrackerService.Api.ViewModels.Tasks
{
    public class CreateTaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskType Type { get; set; }
        public string ParentTaskId { get; set; }
        public DateTime? DueDate { get; set; }
        public List<string> DocumentIdList { get; set; }
    }
}
