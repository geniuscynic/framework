using System.Data;

namespace DoCare.Core.Database.Imp.Operate.OracleOperate
{
    public class OracleSqlSaveable<T, TEntity>  : Saveable<T, TEntity>
    {
       

        public OracleSqlSaveable(IDbConnection connection, TEntity model): base(connection, model)
        {
           
        }

    }
}
