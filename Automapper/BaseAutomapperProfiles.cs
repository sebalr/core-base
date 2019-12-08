using AutoMapper;
using basiTodo.Infraestructure.DTOs;
using CoreBase.Entities;

namespace CoreBase.Automapper
{
    public class BaseAutomapperProfiles : Profile
    {
        public BaseAutomapperProfiles()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
