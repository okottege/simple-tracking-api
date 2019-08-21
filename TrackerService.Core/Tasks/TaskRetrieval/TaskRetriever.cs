using System.Threading.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;
using TrackerService.Core.Repositories;

namespace TrackerService.Core.Tasks.TaskRetrieval
{
    public class TaskRetriever : ITaskRetriever
    {
        private readonly ITaskRetrievalRepository repo;

        public TaskRetriever(ITaskRetrievalRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ITaskList> Search(ITaskFilterOptions options)
        {
            return await repo.GetTasks(options);
        }
    }
}
