using System.Threading.Tasks;
using TrackerService.Core.CoreDomain.Tasks.Definitions;

namespace TrackerService.Core.Tasks.TaskCreation
{
    public interface ITaskCreator
    {
        Task<string> CreateTask(ITask task);
    }
}