using System.Data;
using System.Data.Common;

namespace SimpleNet.Data.Repository.Contracts
{
    public interface ISyncDataAccess
    {

        int ExecuteNonQuery(DbConnection connection, string commandText, CommandType commandType, DbParameter[] parameters,  DbTransaction transaction = null);
        object ExecuteScalar(DbConnection connection, string commandText, CommandType commandType, DbParameter[] parameters, DbTransaction transaction = null);
        DataTable Read(DbConnection connection, string commandText, CommandType commandType, DbParameter[] parameters, DbTransaction transaction = null);
        

        int ExecuteNonQuery(string commandText, CommandType commandType, DbParameter[] parameters);
        object ExecuteScalar(string commandText, CommandType commandType, DbParameter[] parameters);
        DataTable Read(string commandText, CommandType commandType, DbParameter[] parameters);    


        DataTable ReadSql(string commandText, DbParameter[] parameters);
        DataTable ReadProc(string commandText, DbParameter[] parameters);
    }
}