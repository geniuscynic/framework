using System.Data;

namespace DoCare.Zkzx.Core.Database.Imp.Operate.MySqlOperate
{
    public class MySqlQueryable<T> : Queryable<T>
    {
        public MySqlQueryable(IDbConnection connection):base(connection)
        {
            

        }

      
    }
}
