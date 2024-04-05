using Classifieds.Data.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Classifieds.Data.Configuration
{
    public class RoleSeedConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "1DE4BA65-2C7D-4CE6-A3FF-54BB22F81806",
                    Name = Roles.Customer,
                    NormalizedName = Roles.Customer.ToUpper()
                },
                new IdentityRole
                {
                    Id = "D8F9E330-1012-47BF-B1E4-603315A12049",
                    Name = Roles.Administrator,
                    NormalizedName = Roles.Administrator.ToUpper()
                }
            );
        }
    }
}