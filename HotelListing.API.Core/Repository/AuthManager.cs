using AutoMapper;
using HotelListingAPI.Contracts;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListingAPI.Repository
{
    public class AuthManager : IAuthManager
    {
        
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private ApiUser _user;

        

        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";
        


        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration, ILogger<AuthManager> logger)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
            this._logger = logger;
        }

        public  async Task<string> CreateRefreshToken()
        {
            //Remove the old one for the user 
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);

            //Create a new one
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);

            //Setted to the db
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);

            return newRefreshToken;

        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;

            _user = await _userManager.FindByNameAsync(username);

            if(_user == null || _user.Id != request.UserId) 
            {
                return null;
            }

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken,request.RefreshToken);

            if(isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()

                };
            }

            //If token is not valid
            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }



        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _logger.LogInformation($"Looking for user with email {loginDto.Email}"); 

            //Find the user
            _user = await _userManager.FindByEmailAsync(loginDto.Email);

            //Validation if the specific password matches to the User
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

            //If user is not or valid not succeed
            if (_user == null || isValidUser == false)
            {
                _logger.LogWarning($"User with email {loginDto.Email} was not found");
                return null;
            }

            var token = await GenerateToken();
            _logger.LogInformation($"Token Generating succefylly for the {loginDto.Email} | Token : {token}");

            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
            

        }

        public async Task<IEnumerable<IdentityError>> Register (ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);

            //Email will be Username
            _user.UserName = _user.Email;

            //Encrypt the user password for the user
            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }

            
            return result.Errors;

        }

        public async Task<IEnumerable<IdentityError>> RegisterAdmin(ApiUserDto userDto)
        {
            var _user = _mapper.Map<ApiUser>(userDto);

            //Email will be Username
            _user.UserName = _user.Email;

            //Encrypt the user password for the user
            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "Adminstrator");
            }


            return result.Errors;
        }

       


        //Generate the Token
        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);

            var rolesClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            //Bring the claims if exists in db for that user
            var userClaims = await _userManager.GetClaimsAsync(_user);

            //Generare information about the user
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, Guid.NewGuid().ToString()),
                new Claim("uid", _user.Id),

            }
            .Union(userClaims).Union(rolesClaims);

            //Create Token

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration
                ["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials

                );

            //Return all the above info to a string
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }


}
