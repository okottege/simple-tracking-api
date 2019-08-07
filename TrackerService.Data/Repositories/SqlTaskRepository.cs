using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TrackerService.Common.Exceptions;
using TrackerService.Core.CoreDomain;
using TrackerService.Core.CoreDomain.Tasks.Definitions;
using TrackerService.Core.Repositories;
using TrackerService.Data.DataObjects.Tasks;

namespace TrackerService.Data.Repositories
{
    public class SqlTaskRepository : BaseSqlRepository, ITaskRepository
    {
        private readonly IServiceContext serviceContext;
        private readonly IUserContext userContext;

        public SqlTaskRepository(string connString, IServiceContext serviceContext, IUserContext userContext) : base(connString)
        {
            this.serviceContext = serviceContext;
            this.userContext = userContext;
        }

        public async Task<string> CreateNewTask(ITask task)
        {
            var taskPublicId = task.TaskId ?? Guid.NewGuid().ToString();
            const string insertCommand = @"insert into task(public_id, parent_id, tenant_id, title, [description], due_date, [status], 
                                                            [type], created_date, created_by, modified_date, modified_by)
                                           values(@taskPublicId, @parentTaskId, @TenantId, @Title, @Description, @DueDate, @Status, @Type,
                                                   @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy);
                                           select cast(scope_identity() as bigint);";
            using (var conn = await OpenNewConnection())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    long? parentTaskId = null;
                    if (task.Parent?.TaskId != null)
                    {
                        parentTaskId = await EnsureTaskExists(task.Parent.TaskId, conn, transaction);
                    }

                    var taskInternalId = await conn.ExecuteScalarAsync<long>(insertCommand, new
                    {
                        taskPublicId,
                        parentTaskId,
                        serviceContext.TenantId,
                        task.Title,
                        task.Description,
                        task.DueDate,
                        Status = task.Status.ToString(),
                        Type = task.Type.ToString(),
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = userContext.UserId,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedBy = userContext.UserId
                    }, transaction);

                    await CreateAssignments(taskInternalId, task.Assignments, conn, transaction);
                    await CreateTaskContextItems(taskInternalId, task.ContextItems, conn, transaction);

                    transaction.Commit();
                }
            }
            
            return taskPublicId;
        }

        public async Task UpdateTask(ITask task)
        {
            using (var conn = await OpenNewConnection())
            {
                await EnsureTaskExists(task.TaskId, conn);
                const string UpdateQuery = @"update task set
                                                 title = @Title,
                                                 description = @Description,
                                                 due_date = @DueDate,
                                                 [status] = @Status,
                                                 modified_date = @ModifiedDate,
                                                 modified_by = @ModifiedBy
                                             where public_id = @TaskId and tenant_id = @TenantId";
                await conn.ExecuteAsync(UpdateQuery,
                    new
                    {
                        task.Title,
                        task.Description,
                        task.DueDate,
                        task.Status,
                        ModifiedDate = DateTime.UtcNow,
                        ModifiedBy = userContext.UserId,
                        serviceContext.TenantId
                    });
            }
        }

        public async Task<ITask> GetTask(string taskId)
        {
            using (var conn = await OpenNewConnection())
            {
                var taskInternalId = await EnsureTaskExists(taskId, conn);
                const string SelectQuery = @"
                select id, public_id, title, description, parent_id, due_date, [status], [type], 
                       resolved, created_date, created_by, modified_date, modified_by
                from task where id = @taskInternalId and tenant_id = @TenantId;
                
                select entity_type, [entity_id], created_date, created_by
                from task_assignment where task_id = @taskInternalId;

                select [key], [value], created_date, created_by
                from task_context where task_id = @taskInternalId;";

                var reader = await conn.QueryMultipleAsync(SelectQuery, new {taskInternalId, serviceContext.TenantId});
                var task = reader.Read<PlatformTaskData>().Single();
                task.Assignments = reader.Read<AssignmentData>().Cast<ITaskAssignment>().ToList();
                task.ContextItems = reader.Read<ContextData>().Cast<ITaskContextItem>().ToList();
                return task;
            }
        }

        private async Task CreateAssignments(long taskId, List<ITaskAssignment> assignments, IDbConnection conn, SqlTransaction transaction)
        {
            const string insertQuery = @"insert into task_assignment(task_id, entity_type, [entity_id], created_date, created_by)
                                         values(@taskId, @Type, @EntityId, @CreatedDate, @CreatedBy)";
            foreach (var assignment in assignments ?? new List<ITaskAssignment>())
            {
                await conn.ExecuteAsync(insertQuery,
                    new
                    {
                        taskId, Type = assignment.Type.ToString(), assignment.EntityId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = userContext.UserId
                    }, transaction);
            }
        }

        private async Task CreateTaskContextItems(long taskId, List<ITaskContextItem> contextItems, IDbConnection conn, SqlTransaction transaction)
        {
            const string insertQuery = @"insert into task_context(task_id, [key], [value], created_date, created_by)
                                         values(@taskId, @ContextKey, @ContextValue, @CreatedDate, @CreatedBy)";
            foreach (var ctxItem in contextItems ?? new List<ITaskContextItem>())
            {
                await conn.ExecuteAsync(insertQuery,
                    new
                    {
                        taskId,
                        ctxItem.ContextKey,
                        ctxItem.ContextValue,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = userContext.UserId
                    }, transaction);
            }
        }

        private async Task<long> EnsureTaskExists(string taskId, IDbConnection conn, SqlTransaction transaction = null)
        {
            var task = (await conn.QueryAsync<PlatformTaskData>(
                "select * from task where public_id = @taskId and tenant_id = @TenantId",
                new {taskId, serviceContext.TenantId}, transaction)).FirstOrDefault();
            if(task == null) throw new EntityNotFoundException("Task", taskId);

            return task.id;
        }
    }
}
