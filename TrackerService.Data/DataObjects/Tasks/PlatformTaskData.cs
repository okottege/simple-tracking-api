using System;
using System.Collections.Generic;
using TrackerService.Core.CoreDomain;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Data.DataObjects.Tasks
{
    internal class PlatformTaskData : IDocumentSignatureTask
    {
        public string TaskId => public_id;
        public string Title => title;
        public string Description => description;
        public DateTime? DueDate => due_date?.Date == DateTime.MinValue ? null : due_date?.Date.ToUniversalTime();
        public TaskStatus Status => EnumerationExtensions.FromString<TaskStatus>(status);
        public TaskType Type => EnumerationExtensions.FromString<TaskType>(type);
        public ITask Parent { get; set; }
        public List<ITask> Children { get; set; } = new List<ITask>();
        public List<ITaskAssignment> Assignments { get; set; } = new List<ITaskAssignment>();
        public List<ITaskContextItem> ContextItems { get; set; } = new List<ITaskContextItem>();
        public string CreatedBy => created_by;
        public DateTime CreatedDate => created_date;
        public string ModifiedBy => modified_by;
        public DateTime ModifiedDate => modified_date;
        public bool Resolved => resolved == true;
        public List<string> DocumentIdList { get; set; } = new List<string>();

        internal long id { get; set; }
        public string public_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool? resolved { get; set; }
        public DateTime? due_date { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public long parent_id { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }
}
