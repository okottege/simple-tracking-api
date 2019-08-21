using Microsoft.AspNetCore.Mvc;

namespace TrackerService.Api.ViewModels.Tasks
{
    public class TaskSearchViewModel
    {
        [FromQuery(Name = "createdBy")]
        public string CreatedById { get; set; }
        [FromQuery(Name = "status")]
        public string Status { get; set; }
        [FromQuery(Name = "assignedTo")]
        public string AssignedToId { get; set; }
    }
}
