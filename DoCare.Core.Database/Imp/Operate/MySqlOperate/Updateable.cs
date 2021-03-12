using System.Data;

namespace DoCare.Core.Database.Imp.Operate.MySqlOperate
{
    public class MySqlUpdateable<T> : Updateable<T>
    {
        public MySqlUpdateable(IDbConnection connection) : base(connection)
        {


        }
    }
}
