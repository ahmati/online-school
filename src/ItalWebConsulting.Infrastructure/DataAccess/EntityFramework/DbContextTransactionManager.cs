using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ItalWebConsulting.Infrastructure.DataAccess.EntityFramework
{
    public abstract class DbContextTransactionManager<TContext> where TContext : DbContextBase//: IDbTransactionManager<TContext> where TContext : DbContextBase
    {
        private readonly TContext context;
        private const IsolationLevel defaultIsolationLevel = IsolationLevel.ReadCommitted;
        //private IDbConnection Connection { get => Connection; }
        private IDbContextTransaction transaction = null;
        private int counter = 0;
        protected DbContextTransactionManager(TContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private void CloseTransaction(bool commit)
        {
            if (counter > 1 && commit)
                return; // se counter è maggiore di uno vuol dire che ho una transazione richiamata da un'altra transazione, la commit la faccio dal padre

            //Nel caso di un Rollback lo eseguo sempre
            if (transaction != null)
            {
                if (commit)
                    transaction.Commit();
                else
                    transaction.Rollback();

                transaction.Dispose();
                transaction = null;
            }
            counter = 0;
        }

        private void OpenTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (transaction == null)
            {
                context.StartConversation();
                transaction = context.Database.BeginTransaction();
                counter = 0;
            }

            counter++;
        }


        public TResult Transaction<TResult>(Func<TResult> func, bool commit)
        {
            return Transaction<TResult>(func, commit, defaultIsolationLevel);
        }
        public TResult Transaction<TResult>(Func<TResult> func, IsolationLevel isolationLevel)
        {
            return Transaction<TResult>(func, true, isolationLevel);
        }

        public TResult Transaction<TResult>(Func<TResult> func)
        {
            return Transaction<TResult>(func, true, defaultIsolationLevel);
        }

        public TResult Transaction<TResult>(Func<TResult> func, bool commit, IsolationLevel isolationLevel)
        {
            TResult result;
            try
            {
                OpenTransaction(isolationLevel);
                result = func.Invoke();
                CloseTransaction(commit);

            }
            catch (Exception)
            {
                CloseTransaction(false);
                throw;
            }
            return result;
        }


        public void Transaction(Action action, bool commit, IsolationLevel isolationLevel)
        {
            Transaction(() => { action.Invoke(); return false; }, commit, isolationLevel);
        }

        public void Transaction(Action action, bool commit)
        {
            Transaction(action, commit, defaultIsolationLevel);
        }

        public void Transaction<TResult>(Action action, IsolationLevel isolationLevel)
        {
            Transaction(action, true, isolationLevel);
        }

        public void Transaction(Action action)
        {
            Transaction(action, true, defaultIsolationLevel);
        }



    }
}
