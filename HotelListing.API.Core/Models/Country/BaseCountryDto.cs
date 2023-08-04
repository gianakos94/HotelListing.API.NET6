using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace HotelListingAPI.Models.Country
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
