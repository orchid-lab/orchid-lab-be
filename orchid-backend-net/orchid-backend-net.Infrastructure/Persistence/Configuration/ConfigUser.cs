using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using orchid_backend_net.Domain.Entities;

namespace orchid_backend_net.Infrastructure.Persistence.Configuration
{
    internal class ConfigUser : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }
    }
}
