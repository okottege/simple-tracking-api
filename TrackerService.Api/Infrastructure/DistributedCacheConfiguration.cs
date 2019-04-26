using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TrackerService.Api.Infrastructure
{
    public static class DistributedCacheConfiguration
    {
        public static void AddDistributedCache(this IServiceCollection services, IHostingEnvironment env,
            IConfiguration config)
        {
            if (env.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddDistributedRedisCache(options =>
                    {
                        options.Configuration = config.GetConnectionString("RedisCache");
                        options.InstanceName = "timesheeting-test";
                    });
            }
        }
    }
}
