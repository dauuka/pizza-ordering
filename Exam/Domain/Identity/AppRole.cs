using ee.itcollege.dauuka.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppRole : IdentityRole<int>, IBaseEntity
    {
        
    }
}