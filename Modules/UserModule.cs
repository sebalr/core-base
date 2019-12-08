using basiTodo.Infraestructure.DTOs;
using Carter.ModelBinding;
using Carter.Response;
using CoreBase.Entities;
using CoreBase.Persistance.finders;
using CoreBase.Services;

namespace CoreBase.Modules
{
    public class UserModule : BaseApiModule<User, UserDTO>
    {
        public UserModule(IBaseModuleService baseModuleService, IUserService userService,
            IUserFinder userFinder) : base("user", baseModuleService, userFinder)
        {
            Post("/login", async ctx =>
            {
                var userDTO = await ctx.Request.Bind<UserDTO>();

                var user = userService.Authenticate(userDTO.Username, userDTO.Password);

                if (user == null)
                {
                    ctx.Response.StatusCode = 400;
                    await ctx.Response.AsJson(new { message = "Username or password is incorrect" });
                }

                await baseModuleService.RespondWithEntitiyDTO(ctx, user);
            });

        }
    }
}
