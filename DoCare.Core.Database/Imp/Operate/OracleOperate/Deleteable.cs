using System.Data;

namespace DoCare.Core.Database.Imp.Operate.OracleOperate
{
    public class OracleDeleteable<T> : Deleteable<T>
    {
        public OracleDeleteable(IDbConnection connection) : base(connection)
        {


        }
    }
}
