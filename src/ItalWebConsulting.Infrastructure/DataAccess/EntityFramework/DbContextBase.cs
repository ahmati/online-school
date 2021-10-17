using ItalWebConsulting.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace ItalWebConsulting.Infrastructure.DataAccess.EntityFramework
{
    public abstract class DbContextBase : DbContext
    {
        public ILogger<DbContextBase> Logger { get; set; }
        private readonly string connectionString;
        public IDbConnection Connection
        {
            get
            {
                return Database.GetDbConnection();
            }
        }
        protected DbContextBase(string connectionString) 
            //: base(connectionString)
        {
            this.connectionString = connectionString;
            
        }
        //protected DbContextBase()
        ////: base(connectionString)
        //{

        //}
        //protected DbContextBase()
        ////: base(connectionString)
        //{

        //}
        //protected DbContextBase(DbContextOptions options)
        //    : base(options)
        //{

        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(connectionString);
        }
        public override void Dispose()
        {

            if (base.Database.CurrentTransaction != null)
            {
                base.Database.CurrentTransaction.Commit();
                //base.Database.CurrentTransaction.Rollback();
                //base.Database.CurrentTransaction.Dispose();
            }
            base.Dispose();
        }
        public void StartConversation()
        {
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();
        }
        public void StopConversation()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
            Dispose();
        }

        //public void EnableSqlLoggin(LogTraceLevel logTraceLevel)
        //{
        //    switch (logTraceLevel)
        //    {
        //        case LogTraceLevel.Info:
        //            Database.Log = log => logger.Info(log);
        //            break;
        //        case LogTraceLevel.Debug:
        //            Database.Log = log => logger.Debug(log);
        //            break;
        //        case LogTraceLevel.Warn:
        //            Database.Log = log => logger.Warn(log);
        //            break;
        //        case LogTraceLevel.Error:
        //            Database.Log = log => logger.Error(log);
        //            break;
        //        case LogTraceLevel.Fatal:
        //            Database.Log = log => logger.Fatal(log);
        //            break;
        //        case LogTraceLevel.StopLogging:
        //            Database.Log = null;
        //            break;
        //        default:
        //            throw new ArgumentException("Tipo LogTraceLevel in input non gestito: " + logTraceLevel);
        //    }


        //}
    }
}
