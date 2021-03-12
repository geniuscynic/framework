using System.Data;

namespace DoCare.Core.Database.Imp.Operate.OracleOperate
{
    public class OracleQueryable<T> : Queryable<T>
    {
        public OracleQueryable(IDbConnection connection):base(connection)
        {
            

        }

      
    }
}
