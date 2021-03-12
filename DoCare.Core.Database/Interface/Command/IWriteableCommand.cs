using System.Threading.Tasks;

namespace DoCare.Core.Database.Interface.Command
{
    public interface IWriteableCommand   {
        Task<int> Execute();
    }
}
