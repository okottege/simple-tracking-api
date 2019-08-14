using Microsoft.Extensions.Configuration;
using TrackerService.Core;

namespace TrackerService.Api.Infrastructure
{
    public class DataAccessConfiguration : IDataAccessConfiguration
    {
        public DataAccessConfiguration(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("SimpleTaxDB");
        }

        public string ConnectionString { get; }
    }
}
