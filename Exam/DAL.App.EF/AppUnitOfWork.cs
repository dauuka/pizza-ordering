using Contracts.DAL.App;
using ee.itcollege.dauuka.Contracts.DAL.Base.Helpers;
using ee.itcollege.dauuka.DAL.Base.EF;

namespace DAL.App.EF
{
    public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
    {
        public AppUnitOfWork(AppDbContext dataContext, IBaseRepositoryProvider repositoryProvider) : base(dataContext,
            repositoryProvider)
        {
        }
    }
}