using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CoreBase.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureFromAllAssembly(this ModelBuilder modelBuilder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var typesToRegister = assembly.GetTypes().Where(t => t.GetInterfaces()
                    .Any(gi => gi.IsGenericType && gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))).ToList();

                foreach (var type in typesToRegister)
                {
                    dynamic configurationInstance = Activator.CreateInstance(type);
                    modelBuilder.ApplyConfiguration(configurationInstance);
                }
            }
        }
    }
}
