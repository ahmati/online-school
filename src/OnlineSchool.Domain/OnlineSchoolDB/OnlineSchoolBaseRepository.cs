using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;
using ItalWebConsulting.Infrastructure.Logging;
using OnlineSchool.Domain.OnlineSchoolDB.EF.Context;


namespace OnlineSchool.Domain.OnlineSchoolDB
{
    public class OnlineSchoolBaseRepository : RepositoryBase<OnlineSchoolDbContext>
    {
        public new ILoggerManager Logger { get; set; }
    }
}
