using System;
using System.Security.Principal;

namespace CoreBase.Login
{
    internal class AuthUser : IIdentity
    {
        public string AuthenticationType => "JWT";
        public bool IsAuthenticated { get; }
        public string Name { get; }
        public string Login { get; }
        public Guid Id { get; }

        public AuthUser(string name, string login, Guid id)
        {
            Name = name;
            Login = login;
            Id = id;
            IsAuthenticated = true;
        }
    }
}
