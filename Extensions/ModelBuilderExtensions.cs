using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CoreBase.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureFromAllAssembly(this ModelBuilder modelBuilder)
        {
            var applyGenericMethod = typeof(ModelBuilder).GetMethods().Where(m => m.Name == "ApplyConfiguration" && m.GetParameters().First().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>).GetGenericTypeDefinition()).FirstOrDefault();
            // replace GetExecutingAssembly with assembly where your configurations are if necessary
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes()
                        .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters))
                {
                    // use type.Namespace to filter by namespace if necessary
                    foreach (var iface in type.GetInterfaces())
                    {
                        // if type implements interface IEntityTypeConfiguration<SomeEntity>
                        if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                        {
                            // make concrete ApplyConfiguration<SomeEntity> method
                            var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                            // and invoke that with fresh instance of your configuration type
                            applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type) });
                            break;
                        }
                    }
                }
            }
        }
    }
}
