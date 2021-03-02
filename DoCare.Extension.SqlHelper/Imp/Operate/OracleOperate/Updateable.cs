using System.Data;

namespace DoCare.Extension.SqlHelper.Imp.Operate.OracleOperate
{
    public class OracleUpdateable<T> : Updateable<T>
    {
        public OracleUpdateable(IDbConnection connection) : base(connection)
        {


        }
    }
}
