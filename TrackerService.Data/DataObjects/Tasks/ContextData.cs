using System;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Data.DataObjects.Tasks
{
    internal class ContextData : ITaskContextItem
    {
        public string ContextKey => key;
        public string ContextValue => value;
        public DateTime CreatedDate => created_date.ToUniversalTime();
        public string CreatedBy => created_by;

        public string key { get; set; }
        public string value { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
    }
}
