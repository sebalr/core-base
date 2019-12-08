using CoreBase.DTOs;

namespace basiTodo.Infraestructure.DTOs
{
    public class UserDTO : EntityDTO
    {
        public int? UserTypeId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}
