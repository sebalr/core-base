using System;
using Carter;
using Carter.ModelBinding;
using Carter.Request;
using CoreBase.DTOs;
using CoreBase.Entities;
using CoreBase.Extensions;
using CoreBase.Persistance;
using CoreBase.Services;
using Microsoft.AspNetCore.Routing;

namespace CoreBase.Modules
{

    public abstract class BaseApiModule<TEntity, TDTO> : CarterModule
    where TEntity : Entity
    where TDTO : EntityDTO
    {

        protected Action<TEntity, TDTO> BeforeInsertNewEntity;

        public BaseApiModule(string route, IBaseModuleService baseModuleService, IBaseFinder<TEntity> baseFinder) : base(route)
        {
            Get("/", async ctx =>
            {
                var a = ctx.Response;
                var entities = baseFinder.Get();

                await baseModuleService.RespondWithListOfEntitiesDTO<TEntity, TDTO>(ctx, entities);
            });

            Get("/{id:int}", async ctx =>
            {
                var entity = this.GetById<TEntity>(ctx.GetRouteData().As<int>("id"), baseFinder);

                await baseModuleService.RespondWithEntitiyDTO<TEntity, TDTO>(ctx, entity);
            });

            Post("/", async ctx =>
            {
                var entityDTO = await ctx.Request.Bind<TDTO>();
                var entity = baseModuleService.ConvertToEntity<TDTO, TEntity>(entityDTO);

                BeforeInsertNewEntity?.Invoke(entity, entityDTO);

                baseFinder.Insert(entity);

                await baseModuleService.RespondWithEntitiyDTO<TEntity, TDTO>(ctx, entity);
            });

            Put("/{id:int}", async ctx =>
            {
                var entityDTO = await ctx.Request.Bind<TDTO>();

                var entity = this.GetById<TEntity>(ctx.GetRouteData().As<int>("id"), baseFinder);

                entity = baseModuleService.ConvertToEntity<TDTO, TEntity>(entityDTO, entity);

                baseFinder.Update(entity);

                await baseModuleService.RespondWithEntitiyDTO<TEntity, TDTO>(ctx, entity);
            });
        }
    }
}
