
using MVVU_Data.Dapper.Interfaces;

namespace MVVU_Data.Dapper
{
    public interface IDapperContext
    {
        IQueryHelper QueryHelper { get; }
        IProcedureHelper ProcedureHelper { get; }
    }
}
