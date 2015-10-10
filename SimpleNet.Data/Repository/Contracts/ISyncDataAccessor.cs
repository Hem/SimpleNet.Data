using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SimpleNet.Data.Mapper;

namespace SimpleNet.Data.Repository.Contracts
{
    public interface ISyncDataAccessor
    {
        IEnumerable<T> Read<T>(IRowMapper<T> mapper, string commandText, CommandType commandType, DbParameter[] parameters);
        
        IEnumerable<T> Read<T>(DbConnection connection, IRowMapper<T> mapper, string commandText, CommandType commandType, DbParameter[] parameters, DbTransaction transaction = null);

    }
}