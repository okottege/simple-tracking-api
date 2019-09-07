using Microsoft.Extensions.DependencyInjection;
using TrackerService.Services.Users;

namespace TrackerService.Services
{
    public static class DependencyConfig
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserProfileCache, UserProfileCache>();
        }
    }
}
