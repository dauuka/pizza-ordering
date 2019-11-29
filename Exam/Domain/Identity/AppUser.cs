using System.Collections.Generic;
using ee.itcollege.dauuka.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppUser : IdentityUser<int>, IBaseEntity
    {
        public ICollection<FullOrder> FullOrders { get; set; }
    }
}