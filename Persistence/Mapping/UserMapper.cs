using CoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreBase.Persistance.Mapping
{
    public class UserMapper : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User")
                .Property(x => x.Username).IsRequired();

            builder.Property(x => x.Password).IsRequired();

        }
    }
}
