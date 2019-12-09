using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Carter.Response;
using CoreBase.DTOs;
using CoreBase.Entities;
using Microsoft.AspNetCore.Http;

namespace CoreBase.Services
{
    public interface IBaseModuleService
    {
        Task RespondWithListOfEntitiesDTO<TEntity, TDTO>(HttpContext Ctx, IEnumerable<TEntity> Entities)
        where TEntity : Entity
        where TDTO : EntityDTO;

        Task RespondWithListOfEntitiesDTO<TDTO>(HttpContext Ctx, IList<TDTO> DTOs)
        where TDTO : EntityDTO;

        Task RespondWithEntitiyDTO<TEntity, TDTO>(HttpContext Ctx, TEntity Entity)
        where TEntity : Entity
        where TDTO : EntityDTO;

        Task RespondWithEntitiyDTO<TDTO>(HttpContext Ctx, TDTO DTO)
        where TDTO : EntityDTO;

        IList<TDTO> ConvertToDTOs<TEntity, TDTO>(IEnumerable<TEntity> Entities)
        where TEntity : Entity
        where TDTO : EntityDTO;

        TDTO ConvertToDTO<TEntity, TDTO>(TEntity Entity)
        where TEntity : Entity
        where TDTO : EntityDTO;

        TEntity ConvertToEntity<TDTO, TEntity>(TDTO DTO, TEntity Entity = null)
        where TEntity : Entity
        where TDTO : EntityDTO;
    }

    public class BaseModuleService : IBaseModuleService
    {
        private IMapper _mapper;

        public BaseModuleService(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public Task RespondWithListOfEntitiesDTO<TEntity, TDTO>(HttpContext Ctx, IEnumerable<TEntity> Entities)
        where TEntity : Entity
        where TDTO : EntityDTO
        {
            var entitiesDTO = ConvertToDTOs<TEntity, TDTO>(Entities);
            return RespondWithListOfEntitiesDTO(Ctx, entitiesDTO);
        }

        public Task RespondWithEntitiyDTO<TEntity, TDTO>(HttpContext Ctx, TEntity Entity)
        where TEntity : Entity
        where TDTO : EntityDTO
        {
            var entityDTO = ConvertToDTO<TEntity, TDTO>(Entity);

            return RespondWithEntitiyDTO(Ctx, entityDTO);
        }

        public Task RespondWithListOfEntitiesDTO<TDTO>(HttpContext Ctx, IList<TDTO> DTOs)
        where TDTO : EntityDTO
        {
            var dataDto = new DataDTO<TDTO>(DTOs);
            return Ctx.Response.AsJson(dataDto);
        }

        public async Task RespondWithEntitiyDTO<TDTO>(HttpContext Ctx, TDTO DTO)
        where TDTO : EntityDTO
        {
            await Ctx.Response.AsJson(DTO);
        }

        public TDTO ConvertToDTO<TEntity, TDTO>(TEntity Entity)
        where TEntity : Entity
        where TDTO : EntityDTO
        {
            return this._mapper.Map<TEntity, TDTO>(Entity);
        }

        public IList<TDTO> ConvertToDTOs<TEntity, TDTO>(IEnumerable<TEntity> Entities)
        where TDTO : EntityDTO
        where TEntity : Entity
        {
            var entitiesDTO = new List<TDTO>();
            foreach (TEntity entity in Entities)
            {
                if (entity is ISoftDeletable && ((ISoftDeletable) entity).Deleted) continue;
                entitiesDTO.Add(this._mapper.Map<TEntity, TDTO>(entity));
            }
            return entitiesDTO;
        }

        public TEntity ConvertToEntity<TDTO, TEntity>(TDTO DTO, TEntity Entity = null)
        where TEntity : Entity
        where TDTO : EntityDTO
        {
            TEntity entity = default(TEntity);
            if (Entity == null)
                entity = this._mapper.Map<TDTO, TEntity>(DTO);
            else
                entity = this._mapper.Map<TDTO, TEntity>(DTO, Entity);

            return entity;
        }

    }
}
