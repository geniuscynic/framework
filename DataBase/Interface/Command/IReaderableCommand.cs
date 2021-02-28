using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoCare.Extension.DataBase.Interface.Command
{
    public interface IReaderableCommand<T>
    {
        Task<IEnumerable<T>> ExecuteQuery();

        Task<T> ExecuteFirst();

        Task<T> ExecuteFirstOrDefault();

        Task<T> ExecuteSingle();

        Task<T> ExecuteSingleOrDefault();
    }
}
