using System;
using System.Data;

namespace ItalWebConsulting.Infrastructure.DataAccess.AdoNet
{
    public interface IDbTransactionManager //<TContext> where TContext : DbContextBase
    {
        TResult Transaction<TResult>(Func<TResult> func, bool commit);
        TResult Transaction<TResult>(Func<TResult> func, IsolationLevel isolationLevel);
        TResult Transaction<TResult>(Func<TResult> func);
        TResult Transaction<TResult>(Func<TResult> func, bool commit, IsolationLevel isolationLevel);
        void Transaction(Action action, bool commit, IsolationLevel isolationLevel);
        void Transaction(Action action, bool commit);
        void Transaction<TResult>(Action action, IsolationLevel isolationLevel);
        void Transaction(Action action);

    }
}
