using Microsoft.Extensions.Configuration;

namespace TrackerService.Common.Configuration
{
    public static class ConfigurationExtensions
    {
        public static UserManagementConfig GetUserManagementOptions(this IConfiguration config)
        {
            return config.GetSection<UserManagementConfig>();
        }

        public static AuthenticationConfig GetAuthenticationOptions(this IConfiguration config)
        {
            return config.GetSection<AuthenticationConfig>();
        }

        private static T GetSection<T>(this IConfiguration config) => config.GetSection(typeof(T).Name).Get<T>();
    }
}
