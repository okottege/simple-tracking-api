using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrackerService.Core.CoreDomain.Tasks;

namespace TrackerService.Api.ViewModels.Tasks
{
    public class CreateTaskViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskType Type { get; set; }
        public string ParentTaskId { get; set; }
        public DateTime? DueDate { get; set; }
        public List<string> DocumentIdList { get; set; }
        public List<AssignmentViewModel> Assignments { get; set; } = new List<AssignmentViewModel>();
        public List<ContextViewModel> ContextItems { get; set; } = new List<ContextViewModel>();
    }
}
