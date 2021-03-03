using System.Data;

namespace DoCare.Extension.SqlHelper.Imp.Operate.SqlOperate
{
    public class SqlUpdateable<T> : Updateable<T>
    {
        public SqlUpdateable(IDbConnection connection) : base(connection)
        {


        }
    }
}
