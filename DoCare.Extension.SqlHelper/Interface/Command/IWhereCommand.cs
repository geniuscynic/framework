using System;
using System.Linq.Expressions;
using System.Text;

namespace DoCare.Extension.SqlHelper.Interface.Command
{
    interface IWhereCommand<T>
    {
        void Where(Expression<Func<T, bool>> predicate);

        void Where(string whereExpression);

        void Where<TResult>(string whereExpression, Expression<Func<TResult>> predicate);

        StringBuilder Build(bool ignorePrefix = true);
    }
}
