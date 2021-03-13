using System.Data;

namespace DoCare.Zkzx.Core.Database.Imp.Operate.SqlOperate
{
    public class SqlQueryable<T> : Queryable<T>
    {
        public SqlQueryable(IDbConnection connection):base(connection)
        {
            

        }

      
    }
}
