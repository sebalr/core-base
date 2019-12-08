using AutoMapper;
using CoreBase.DTOs;
using CoreBase.Entities;
using CoreBase.Persistance;

namespace CoreBase.Automapper
{
    public class EntityResolver<TDTO, TModel> : IMemberValueResolver<object, object, TDTO, TModel>
        where TDTO : EntityDTO
    where TModel : Entity
    {
        private readonly DatabaseContext _dbContext;

        public EntityResolver(DatabaseContext BaseContext)
        {
            _dbContext = BaseContext;
        }

        public TModel Resolve(object source, object destination, TDTO sourceMember, TModel destinationMember, ResolutionContext Context)
        {
            if (sourceMember == null)
                return null;

            if (sourceMember.Id != 0)
                return _dbContext.Find<TModel>(sourceMember.Id);

            return null;
        }
    }
}
