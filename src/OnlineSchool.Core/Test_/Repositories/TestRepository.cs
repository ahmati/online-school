using ItalWebConsulting.Infrastructure.DataAccess.EntityFramework;

namespace OnlineSchool.Core.Test_.Repositories
{
    public class TestRepository
    //public class TestRepository: MondoInstitutionalBaseRepository
    {
        public int Id { get; set; }

        public int TestContext()
        {
            //var spec = new AsRamoSpecification(null);
            //Expression<Func<Comparto, bool>> ex = e => e.Id == 17;
            //spec.Criteria = ex;
            ////spec.IsPagingEnabled
            //var ls = base.List <Comparto>(spec);
            //var f = base.Context.AsRamo.FirstOrDefault();
            //return base.Count<Comparto>();
            //base.Context.Database.Connection.Open();
            //var cmd = base.Context.Database.Connection.CreateCommand();
            //cmd.CommandText = "SELECT * FROM as_ramo";
            //cmd.ExecuteScalar();
            //return base.Context.AsRamo.Count();
            return 0;
        }
    }
    public interface ITestRepository : IRepositoryBase
    {
        //int Id { get; set; }
        //int TestContext();
    }
}
