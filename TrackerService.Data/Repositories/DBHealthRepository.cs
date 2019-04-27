using System;
using System.Threading.Tasks;
using TrackerService.Data.Contracts;

namespace TrackerService.Data.Repositories
{
    internal class DBHealthRepository : BaseSqlRepository, IDBHealthCheckRepository
    {
        internal DBHealthRepository(string connString) : base(connString)
        {
        }

        public async Task<bool> CanConnectToDatabase()
        {
            const string SqlQuery = "select top 1 EmployeeId from Employee";
            var canConnect = true;
            try
            {
                await QueryAsync<int>(SqlQuery, null);
            }
            catch (Exception)
            {
                canConnect = false;
            }

            return canConnect;
        }
    }
}
