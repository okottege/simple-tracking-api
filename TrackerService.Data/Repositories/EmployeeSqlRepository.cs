using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using TrackerService.Data.Contracts;
using TrackerService.Data.DataObjects;

namespace TrackerService.Data.Repositories
{
    internal class EmployeeSqlRepository : IEmployeeRepository
    {
        private readonly string connString;

        internal EmployeeSqlRepository(string connString)
        {
            this.connString = connString;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            using (IDbConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                var result = await conn.QueryAsync<Employee>("SELECT * FROM employee");
                return result;
            }
        }
    }
}
