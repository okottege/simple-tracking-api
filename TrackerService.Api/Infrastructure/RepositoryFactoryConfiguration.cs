using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackerService.Core.CoreDomain;
using TrackerService.Data;
using TrackerService.Data.Contracts;

namespace TrackerService.Api.Infrastructure
{
    public static class RepositoryFactoryConfiguration
    {
        public static void AddRepositoryFactory(this IServiceCollection services, IConfiguration config)
        {
            var storageConn = new StorageConnectionInfo(config.GetConnectionString("CloudStorage"), config["StorageConnection:ContainerName"]);
            var serviceProvider = services.BuildServiceProvider();
            var userContext = serviceProvider.GetService<IUserContext>();
            var serviceContext = serviceProvider.GetService<IServiceContext>();

            services.AddTransient<IRepositoryFactory>(provider =>
                new RepositoryFactory(config.GetConnectionString("SimpleTaxDB"), storageConn, serviceContext, userContext));
        }
    }
}
