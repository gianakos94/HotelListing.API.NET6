using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using Microsoft.Build.Execution;

namespace HotelListingAPI.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<CountryDto> GetDetails(int id);
    }
   
}
 