namespace CoreBase.Entities
{
    public class User : Entity
    {
        public int? UserTypeId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
