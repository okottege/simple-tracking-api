using System.Threading.Tasks;

namespace TrackerService.Data.Contracts
{
    public interface IDBHealthCheckRepository
    {
        Task<bool> CanConnectToDatabase();
    }
}