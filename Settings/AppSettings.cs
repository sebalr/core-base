using System;
namespace CoreBase.Settings
{
    public interface IAppSettings
    {
        AuthSettings AuthSettings { get; }
        DbSettings DbSettings { get; }
    }

    public class AppSettings : IAppSettings
    {
        public AuthSettings AuthSettings { get; set; }
        public DbSettings DbSettings { get; set; }
    }

    public class AuthSettings
    {
        public string SecretKey { get; set; }

        public string ClientId { get; set; }
    }

    public class DbSettings
    {
        public string ConnectionString { get; set; }
    }
}
