using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace TrackerService.Data.Repositories
{
    internal class BaseSqlRepository
    {
        private readonly string connString;

        protected BaseSqlRepository(string connString)
        {
            this.connString = connString;
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object @params)
        {
            using (IDbConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                return await conn.QueryAsync<T>(sql, @params);
            }
        }

        protected async Task<int> Insert(string sql, object @params)
        {
            var id = (await QueryAsync<int>(sql, @params)).Single();
            return id;
        }

        protected async Task<bool> ExecuteCommand(string sql, object @params)
        {
            using (IDbConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                var numRows = await conn.ExecuteAsync(sql, @params);
                return numRows > 0;
            }
        }
    }
}
