using System.Data;

namespace DoCare.Core.Database.Imp.Operate.OracleOperate
{
    public class OracleUpdateable<T> : Updateable<T>
    {
        public OracleUpdateable(IDbConnection connection) : base(connection)
        {


        }
    }
}
