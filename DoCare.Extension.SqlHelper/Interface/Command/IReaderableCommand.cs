using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoCare.Extension.SqlHelper.Interface.Command
{
    public interface IReaderableCommand<T>
    {
        Task<IEnumerable<T>> ExecuteQuery();

        Task<T> ExecuteFirst();

        Task<T> ExecuteFirstOrDefault();

        Task<T> ExecuteSingle();

        Task<T> ExecuteSingleOrDefault();

        Task<(IEnumerable<T> data, int total)> ToPageList(int pageIndex, int pageSize);
    }
}
