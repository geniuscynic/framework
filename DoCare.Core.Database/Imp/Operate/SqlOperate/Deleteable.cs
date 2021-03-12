using System.Data;

namespace DoCare.Core.Database.Imp.Operate.SqlOperate
{
    public class SqlDeleteable<T> : Deleteable<T>
    {
        public SqlDeleteable(IDbConnection connection) : base(connection)
        {


        }
    }
}
