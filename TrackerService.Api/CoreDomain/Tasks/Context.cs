using System;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Api.CoreDomain.Tasks
{
    public class Context : ITaskContextItem
    {
        public string ContextKey { get; set; }
        public string ContextValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
