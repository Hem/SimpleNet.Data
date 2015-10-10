using System.Data.Common;
using SimpleNet.Data.Connection;

namespace SimpleNet.Data.Repository.Contracts
{
    public interface ISimpleDataAccess : ISyncDataAccess, ISyncDataAccessor, IAsyncDataAccess, IAsyncDataAccessor, ISimpleDbParameterProvider
    {
        DbConnection GetConnection();

    }

}