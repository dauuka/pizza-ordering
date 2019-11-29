using ee.itcollege.dauuka.Contracts.DAL.Base;

namespace Domain
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}