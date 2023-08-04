using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingAPI.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
 
            builder.HasData(
                new IdentityRole
                {
                    Name = "Adminstrator",
                    NormalizedName = "ADMINSTRATOR"
                },
                new IdentityRole
                {
                     Name = "User",
                     NormalizedName = "USER"
                }
              );
        }
    }
}
