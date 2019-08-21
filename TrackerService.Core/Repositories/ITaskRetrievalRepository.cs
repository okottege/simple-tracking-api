using System.Threading.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Core.Repositories
{
    public interface ITaskRetrievalRepository
    {
        Task<ITaskList> GetTasks(ITaskFilterOptions filter);
    }
}