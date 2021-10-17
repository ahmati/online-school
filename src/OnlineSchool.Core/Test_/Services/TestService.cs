using ItalWebConsulting.Infrastructure.BusinessLogic;
using OnlineSchool.Core.Test_.Repositories;

namespace OnlineSchool.Core.Test_.Services
{
    public class TestService : CoreBase, ITestService
    {
        //private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        //public IMondoInstitutionalTransactionManager MondoInstitutionalTransactionManager { get; set; }


        public ITestRepository TestRepository { get; set; }
        public int Prova { get; set; }

        public int TestContext()
        {
            //return MondoInstitutionalTransactionManager.Transaction(() =>
            //{

            //    return TestRepository.TestContext();
            //});

            //return TestRepository.TestContext();
            return 0;

        }
    }

    public class TestService2 : CoreBase
    {
        public int Prova2 { get; set; }
    }

    public interface ITestService
    {
        int Prova { get; set; }
        int TestContext();


    }
}
