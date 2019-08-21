using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TrackerService.Core;
using TrackerService.Core.CoreDomain;
using TrackerService.Core.CoreDomain.Tasks.Definitions;
using TrackerService.Core.Repositories;
using TrackerService.Data.DataObjects.Tasks;

namespace TrackerService.Data.Repositories
{
    public class SqlTaskRetrievalRepository : BaseSqlRepository, ITaskRetrievalRepository
    {
        private readonly IServiceContext serviceContext;

        public SqlTaskRetrievalRepository(IDataAccessConfiguration config, IServiceContext serviceContext) 
            : base(config.ConnectionString)
        {
            this.serviceContext = serviceContext;
        }

        public async Task<ITaskList> GetTasks(ITaskFilterOptions filter)
        {
            const string QueryTemplate = @"
                select
                /**select**/
                from task t
                /**leftjoin**/
                /**where**/
                /**orderby**/
            ";
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Select(
                @"t.public_id, t.title, t.status, t.resolved, t.due_date, t.created_by, 
                  ta.id assignment_id, ta. ta.entity_id, ta.entity_type, 
                  tc.id context_id, tc.[key], tc.value");
            sqlBuilder.LeftJoin("left join task_context tc on t.id = tc.task_id");
            sqlBuilder.LeftJoin("left join task_assignment ta on t.id = ta.task_id");
            sqlBuilder.Where("t.tenant_id = @TenantId", new {serviceContext.TenantId});
            sqlBuilder.OrderBy("t.due_date, t.created_date");
            AddFilterCriteria(filter, sqlBuilder);

            var template = sqlBuilder.AddTemplate(QueryTemplate);

            using (var connection = await OpenNewConnection())
            {
                var taskLookup = new Dictionary<string, PlatformTaskData>();
                var reader = await connection.QueryMultipleAsync(template.RawSql, template.Parameters);
                reader.Read<PlatformTaskData, AssignmentData, ContextData, PlatformTaskData>(
                    (task, assignment, context) => MapTaskSearchResults(task, assignment, context, taskLookup),
                    "assignment_id, context_id");
                return new TaskSearchList {Tasks = taskLookup.Values.Cast<ITask>().ToList()};
            }
        }

        private static void AddFilterCriteria(ITaskFilterOptions filter, SqlBuilder sqlBuilder)
        {
            if (!string.IsNullOrWhiteSpace(filter.AssignedToId))
            {
                sqlBuilder.Where("ta.entity_id = @AssignedToId", new { filter.AssignedToId });
            }

            if (!string.IsNullOrWhiteSpace(filter.CreatedById))
            {
                sqlBuilder.Where("t.created_by_id = @CreatedById", new { filter.CreatedById });
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                sqlBuilder.Where("t.status = @Status", new { filter.Status });
            }
        }

        private static PlatformTaskData MapTaskSearchResults(
            PlatformTaskData task, 
            AssignmentData assignment, 
            ContextData context,
            IDictionary<string, PlatformTaskData> taskLookup)
        {
            if (!taskLookup.TryGetValue(task.TaskId, out var taskInLookup))
            {
                taskInLookup = task;
                taskLookup.Add(task.TaskId, taskInLookup);
            }

            if (assignment != null && taskInLookup.Assignments.Cast<AssignmentData>()
                    .All(a => a.assignment_id != assignment.assignment_id))
            {
                taskInLookup.Assignments.Add(assignment);
            }

            if (context != null && taskInLookup.ContextItems.Cast<ContextData>()
                    .All(c => c.context_id != context.context_id))
            {
                taskInLookup.ContextItems.Add(context);
            }

            return taskInLookup;
        }
    }
}
