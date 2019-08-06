using System.Threading.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Core.Repositories
{
    public interface ITaskRepository
    {
        Task<string> CreateNewTask(ITask task);

        Task UpdateTask(ITask task);

        Task<ITask> GetTask(string taskId);
    }
}