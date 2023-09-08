using AutoMapper;
using HotelListingAPI.Configurations;
using HotelListingAPI.Data;
using HotelListingAPI.Repository;
using System.Drawing.Text;

namespace RepositoryUnitTests
{
    public class RepositoryTests
    {

        private readonly HotelListingDbContext _context;
        private readonly GenericRepository<T> _genericRepository;

        public RepositoryTests()
        {

            var _mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new MapperConfig());
            }).CreateMapper();


           

        }


    }
}