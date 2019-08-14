using System.Threading.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;
using TrackerService.Core.Repositories;

namespace TrackerService.Core.Tasks.TaskCreation
{
    public class TaskCreator : ITaskCreator
    {
        private readonly ITaskRepository taskRepo;

        public TaskCreator(ITaskRepository taskRepo)
        {
            this.taskRepo = taskRepo;
        }

        public async Task<string> CreateTask(ITask task)
        {
            return await taskRepo.CreateNewTask(task);
        }
    }
}
