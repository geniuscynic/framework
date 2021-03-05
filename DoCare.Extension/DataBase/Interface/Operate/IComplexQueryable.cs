using System;
using System.Linq.Expressions;
using DoCare.Extension.DataBase.Interface.Command;

namespace DoCare.Extension.DataBase.Interface.Operate
{
    public interface IComplexQueryable<T> : IReaderableCommand<T>
    {
        IComplexQueryable<T> Where(Expression<Func<T, bool>> predicate);

        IComplexQueryable<T> Where(string whereExpression);

        IComplexQueryable<T> Where<TResult>(string whereExpression, Expression<Func<TResult>> predicate);

        IComplexQueryable<T> OrderBy<TResult>(Expression<Func<T, TResult>> predicate);

        IComplexQueryable<T> OrderByDesc<TResult>(Expression<Func<T, TResult>> predicate);

        IReaderableCommand<TResult> Select<TResult>(Expression<Func<T, TResult>> predicate);


    }

    public interface IComplexQueryable<T1, T2>  : IComplexQueryable<T1>
    {
        IComplexQueryable<T1, T2> Where(Expression<Func<T1, T2, bool>> predicate);

        //IComplexQueryable<T1, T2> Where(string whereExpression);

        //IComplexQueryable<T1, T2> Where<TResult>(string whereExpression, Expression<Func<TResult>> predicate);

        IComplexQueryable<T1, T2> OrderBy<TResult>(Expression<Func<T1, T2, TResult>> predicate);

        IComplexQueryable<T1, T2> OrderByDesc<TResult>(Expression<Func<T1, T2, TResult>> predicate);

    }
}
