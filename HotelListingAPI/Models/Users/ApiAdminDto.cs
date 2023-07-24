using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Models.Users
{
    public class ApiAdminDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


    }
}
