using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace TrackerService.Data.Repositories
{
    public class BaseSqlRepository
    {
        protected readonly string connString;

        protected BaseSqlRepository(string connString)
        {
            this.connString = connString;
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object @params)
        {
            using (var conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                return await conn.QueryAsync<T>(sql, @params);
            }
        }

        protected async Task<T> Insert<T>(string sql, object @params)
        {
            T id = (await QueryAsync<T>(sql, @params)).Single();
            return id;
        }

        protected async Task<bool> ExecuteCommand(string sql, object @params)
        {
            using (var conn = await OpenNewConnection())
            {
                var numRows = await conn.ExecuteAsync(sql, @params);
                return numRows > 0;
            }
        }

        protected async Task<SqlConnection> OpenNewConnection()
        {
            var connection = new SqlConnection(connString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
