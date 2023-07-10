using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        
        {
                 
        }
         
        //Adding tabels to db
        public DbSet<Hotel> hotels { get; set; }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id= 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Greece",
                    ShortName = "GR"
                },
                new Country
                {
                    Id = 3,
                    Name = "Italy",
                    ShortName = "IT"
                }


                );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    CountryId= 1,
                    Rating = 4.5
                },
                 new Hotel
                 {
                     Id = 2,
                     Name = "Comfort Suites",
                     Address = "Goerge Town",
                     CountryId = 2,
                     Rating = 4.3
                 },
                  new Hotel
                  {
                      Id = 3,
                      Name = "Grand Palladium",
                      Address = "Nassua",
                      CountryId = 3,
                      Rating = 4
                  }




                );
        }
    }
}
