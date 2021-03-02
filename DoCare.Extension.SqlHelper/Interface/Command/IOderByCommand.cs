using System;
using System.Linq.Expressions;
using System.Text;

namespace DoCare.Extension.SqlHelper.Interface.Command
{
    interface IOrderByCommand<T>
    {
        void OrderBy<TResult>(Expression<Func<T, TResult>> predicate);

        void OrderByDesc<TResult>(Expression<Func<T, TResult>> predicate);


        StringBuilder Build(bool ignorePrefix = true);
    }
}
