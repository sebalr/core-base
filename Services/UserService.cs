using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using basiTodo.Infraestructure.DTOs;
using CoreBase.Entities;
using CoreBase.Helpers;
using CoreBase.Persistance.finders;
using CoreBase.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoreBase.Services
{
    public interface IUserService
    {
        UserDTO Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {

        private readonly AuthSettings _authSettings;
        private readonly IUserFinder _userFinder;
        private readonly IBaseModuleService _baseService;

        public UserService(IOptions<AuthSettings> authSettings, IUserFinder UserFinder,
            IBaseModuleService BaseService)
        {
            _authSettings = authSettings.Value;
            _userFinder = UserFinder;
            _baseService = BaseService;
        }

        public UserDTO Authenticate(string username, string password)
        {
            var user = _userFinder.getByUsername(username);

            if (user == null)
                return null;

            if (!HashHandle.CheckMatch(user.Password, password))
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userDTO = _baseService.ConvertToDTO<User, UserDTO>(user);
            userDTO.Token = tokenHandler.WriteToken(token);
            userDTO.Password = null;

            return userDTO;
        }

    }
}
