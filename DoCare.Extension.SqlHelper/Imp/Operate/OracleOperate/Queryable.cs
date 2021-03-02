using System.Data;

namespace DoCare.Extension.SqlHelper.Imp.Operate.OracleOperate
{
    public class OracleQueryable<T> : Queryable<T>
    {
        public OracleQueryable(IDbConnection connection):base(connection)
        {
            

        }

      
    }
}
