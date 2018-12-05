using System.Threading.Tasks;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Contracts
{
    public interface IUserRepository
    {
        Task<User> Register(UserRegistration registration);

        Task<User> GetUser(string id);
    }
}
