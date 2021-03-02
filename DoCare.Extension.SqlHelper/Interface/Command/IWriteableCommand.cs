using System.Threading.Tasks;

namespace DoCare.Extension.SqlHelper.Interface.Command
{
    public interface IWriteableCommand   {
        Task<int> Execute();
    }
}
