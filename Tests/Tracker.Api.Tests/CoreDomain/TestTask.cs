using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using TrackerService.Core.CoreDomain.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace Tracker.Api.Tests.CoreDomain
{
    internal class TestTask : ITask
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskType Type { get; set; }
        public ITask Parent { get; set; }
        public List<ITask> Children { get; set; }
        public List<ITaskAssignment> Assignments { get; set; }
        public List<ITaskContextItem> ContextItems { get; set; }
        public List<ITaskDependency> Dependencies { get; set; }

        internal TestTask WithBasicData(string id)
        {
            TaskId = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Title = "Test Title";
            Description = "Test Description";
            DueDate = DateTime.Now.AddDays(7);
            return this;
        }

        internal TestTask WithStatus(TaskStatus status)
        {
            Status = status;
            return this;
        }

        internal TestTask WithNumberOfAssignments(int numAssignments)
        {
            var assignments = new List<ITaskAssignment>();

            for (var i = 0; i < numAssignments; i++)
            {
                var assignment = Substitute.For<ITaskAssignment>();
                assignment.Type.Returns(AssignmentEntityType.User);
                assignment.EntityId.Returns(Guid.NewGuid().ToString());
                assignments.Add(assignment);
            }

            Assignments = assignments;
            return this;
        }

        internal TestTask WithNumberOfContextItems(int numContextItems)
        {
            var contextItems = new List<ITaskContextItem>();

            for (var i = 0; i < numContextItems; i++)
            {
                var contextItem = Substitute.For<ITaskContextItem>();
                contextItem.ContextKey.Returns($"Key_{i+1}");
                contextItem.ContextValue.Returns($"Value_{i+1}");
                contextItems.Add(contextItem);
            }

            ContextItems = contextItems;
            return this;
        }

        internal TestTask WithTaskDependencies(params string[] taskIds)
        {
            Dependencies = taskIds.Select(id =>
            {
                var dependency = Substitute.For<ITaskDependency>();
                dependency.DependsOnTaskId.Returns(id);
                dependency.Type.Returns(DependencyType.Complete);
                return dependency;
            }).ToList();
            return this;
        }
    }
}
