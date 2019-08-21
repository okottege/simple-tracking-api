using System.Threading.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Core.Tasks.TaskRetrieval
{
    public interface ITaskRetriever
    {
        Task<ITaskList> Search(ITaskFilterOptions options);
    }
}