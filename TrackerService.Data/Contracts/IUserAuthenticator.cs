using System.Threading.Tasks;
using TrackerService.Common.Contracts;

namespace TrackerService.Data.Contracts
{
    public interface IUserAuthenticator
    {
        Task<IUserAuthenticationResult> Authenticate(string username, string password);
    }
}