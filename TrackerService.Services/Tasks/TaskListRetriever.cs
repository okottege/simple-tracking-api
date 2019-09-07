using System.Threading.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;
using TrackerService.Core.Repositories;
using TrackerService.Services.DataObjects.Tasks;

namespace TrackerService.Services.Tasks
{
    public class TaskListRetriever
    {
        private readonly ITaskRetrievalRepository repo;

        public TaskListRetriever(ITaskRetrievalRepository repo)
        {
            this.repo = repo;
        }

        public async Task<TaskSearchResult> Search(ITaskFilterOptions options)
        {
            var tasks = await repo.GetTasks(options);
            return new TaskSearchResult();
        }
    }
}