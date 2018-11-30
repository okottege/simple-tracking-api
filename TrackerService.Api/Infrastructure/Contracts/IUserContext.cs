using System.Threading.Tasks;

namespace TrackerService.Api.Infrastructure.Contracts
{
    public interface IUserContext
    {
        string UserId { get; }
        string Email { get; }
        Task<string> GetAccessToken();
        bool IsAdmin { get; }
    }
}