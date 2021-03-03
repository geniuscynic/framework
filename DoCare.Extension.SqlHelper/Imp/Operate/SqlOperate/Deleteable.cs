using System.Data;

namespace DoCare.Extension.SqlHelper.Imp.Operate.SqlOperate
{
    public class SqlDeleteable<T> : Deleteable<T>
    {
        public SqlDeleteable(IDbConnection connection) : base(connection)
        {


        }
    }
}
