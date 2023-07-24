using Microsoft.AspNetCore.Identity;

namespace HotelListingAPI.Data
{
    public class ApiAdmin : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
