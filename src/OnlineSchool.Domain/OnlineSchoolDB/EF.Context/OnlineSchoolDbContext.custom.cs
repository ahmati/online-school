using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;


namespace OnlineSchool.Domain.OnlineSchoolDB.EF.Context
{
    public partial class OnlineSchoolDbContext : DbContextBase
    {
        public OnlineSchoolDbContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
