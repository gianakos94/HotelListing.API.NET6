using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;

namespace HotelListingAPI.Contracts
{
    public interface IAuthManager
    {
        
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);

        Task<IEnumerable<IdentityError>> RegisterAdmin(ApiUserDto userDto);

        Task<AuthResponseDto> Login(LoginDto loginDto);


        //For refreshing tokens
        Task<string> CreateRefreshToken();

        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
       
    }
}
