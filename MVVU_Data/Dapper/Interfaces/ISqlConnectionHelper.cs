
using System.Data;

namespace MVVU_Data.Dapper.Interfaces
{
    public interface ISqlConnectionHelper
    {
        IDbConnection GetDbConnection();
    }
}
