using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HotelListingAPI.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
          
            builder.HasData(
               new Country
               {
                   Id = 31,
                   Name = "Jamaica",
                   ShortName = "JM"
               },
               new Country
               {
                   Id = 32,
                   Name = "Bahamas",
                   ShortName = "BS"
               },
               new Country
               {
                   Id = 33,
                   Name = "Cayman Island",
                   ShortName = "CI"
               }
               );
        }
    }
}
