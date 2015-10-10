using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SimpleNet.Data.Repository.Contracts
{
    public interface IAsyncDataAccess
    {

        Task<DataTable> ReadAsync(DbConnection connection, string commandText, CommandType commandType, DbParameter[] parameters, DbTransaction transaction = null);
        Task<DataTable> ReadAsync(string commandText, CommandType commandType, DbParameter[] parameters);

        Task<DataTable> ReadProcAsync(string commandText, DbParameter[] parameters);
        Task<DataTable> ReadSqlAsync(string commandText, DbParameter[] parameters);

        Task<int> ExecuteNonQueryAsync(DbConnection connection, string commandText, CommandType commandType, DbParameter[] parameters, DbTransaction transaction = null);
        Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, DbParameter[] parameters);


        Task<object> ExecuteScalarAsync(DbConnection connection, string commandText, CommandType commandType, DbParameter[] parameters, DbTransaction transaction = null);
        Task<object> ExecuteScalarAsync(string commandText, CommandType commandType, DbParameter[] parameters);
    }
}