using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingAPI.Data.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 5,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    CountryId = 31,
                    Rating = 4.5
                },
                 new Hotel
                 {
                     Id = 6,
                     Name = "Sanotrini Spa",
                     Address = "Santorini",
                     CountryId = 32,
                     Rating = 4
                 },
                  new Hotel
                  {
                      Id = 7,
                      Name = "Sandals Resort and Spa",
                      Address = "Negril",
                      CountryId = 33,
                      Rating = 4.2
                  }

                );
        }
    }
}
