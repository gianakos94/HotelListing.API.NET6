using AutoMapper;
using HotelListingAPI.Contracts;
using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HotelListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
       
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;
        

        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            this._authManager = authManager;
            this._logger = logger;
          
        }

        // POST : api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Register([FromBody]ApiUserDto apiUserDto)
        {
            _logger.LogInformation($"Registration Attempt for {apiUserDto.Email}");

            
            try
            {
                var errors = await _authManager.Register(apiUserDto);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {

                        ModelState.AddModelError(error.Code, error.Description);

                    }

                    return BadRequest(ModelState);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                 _logger.LogError(ex,$"Registration Attempt for {nameof(Register)} User Registration Attemp");

                return Problem($"Something went wrong in the {nameof(Register)}",statusCode:500);
            }

         }
            

        // POST: api/Account/register/admin
        [HttpPost]
        [Route("register/admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RegisterAdmin([FromBody] ApiUserDto apiUserDto)
        {
            _logger.LogInformation($"Registration Admin Attempt for {apiUserDto.Email}");

           
            try
            {
                var errors = await _authManager.Register(apiUserDto);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        _logger.LogWarning($"Register Adminstrator Attempt fail something goes wrong for {apiUserDto.Email}");
                        ModelState.AddModelError(error.Code, error.Description);

                    }
                    return BadRequest(ModelState);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Registration Attempt for {nameof(Register)} Adminstrator Registration Attemp");

                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }



        // POST : api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {

            var authResponse = await _authManager.Login(loginDto);

            if (authResponse == null)
            {
                _logger.LogInformation($"Login Attempt fail something goes wrong for {loginDto.Email}");
                return Unauthorized();
            }

            return Ok(authResponse);
        }


        // POST : api/Account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            var authResponse = await _authManager.VerifyRefreshToken(request);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);

        }


    }
}
