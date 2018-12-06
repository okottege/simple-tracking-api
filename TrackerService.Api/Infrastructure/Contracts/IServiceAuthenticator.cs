using System.Threading.Tasks;
using TrackerService.Api.Infrastructure.Authentication.Models;

namespace TrackerService.Api.Infrastructure.Contracts
{
    public interface IServiceAuthenticator
    {
        Task<string> AuthenticateAsync();
    }
}
