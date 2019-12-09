using System;
using System.Linq;
using System.Reflection;
using Ardalis.EFCore.Extensions;
using CoreBase.Entities;
using CoreBase.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CoreBase.Persistance
{

    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureFromAllAssembly();

        }

    }
}
