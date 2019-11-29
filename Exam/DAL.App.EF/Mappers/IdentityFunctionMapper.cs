using ee.itcollege.dauuka.Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class IdentityFunctionMapper : IBaseDALMapper
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            return (TOutObject) inObject;
        }
    }
}