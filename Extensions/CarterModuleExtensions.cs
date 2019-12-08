using System.Net;
using Carter;
using CoreBase.Entities;
using CoreBase.Persistance;

namespace CoreBase.Extensions
{
    public static class NancyModuleExtensions
    {
        public static T GetById<T>(this CarterModule Module, int Id, IBaseFinder<T> Finder)
        where T : Entity
        {
            T entity = Finder.GetById(Id);

            if (entity == null)
            {
                throw new HttpException(HttpStatusCode.BadRequest, $"Element with Id = {Id.ToString()} not found");
            }
            else
            {
                return entity;
            }
        }

    }
}
