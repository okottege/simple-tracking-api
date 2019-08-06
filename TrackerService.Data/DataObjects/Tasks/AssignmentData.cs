using System;
using TrackerService.Core.CoreDomain;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Data.DataObjects.Tasks
{
    internal class AssignmentData : ITaskAssignment
    {
        public AssignmentEntityType Type => EnumerationExtensions.FromString<AssignmentEntityType>(entity_type);
        public string EntityId => entity_id;
        public DateTime CreatedDate => created_date.ToUniversalTime();
        public string CreatedBy => created_by;

        public string entity_type { get; set; }
        public string entity_id { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
    }
}
